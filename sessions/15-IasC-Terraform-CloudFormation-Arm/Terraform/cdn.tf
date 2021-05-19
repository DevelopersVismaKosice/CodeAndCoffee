locals {
  s3_origin_id                     = format("cdn-website%s", var.environment == "dev" ? "-dev" : "")
  jira_connector_origin_id         = format("cdn-jira-connector%s", var.environment == "dev" ? "-dev" : "")
  git_connector_origin_id          = format("cdn-git-connector%s", var.environment == "dev" ? "-dev" : "")
  code_quality_connector_origin_id = format("cdn-code-quality-connector%s", var.environment == "dev" ? "-dev" : "")
  aggregator_origin_id             = format("cdn-aggregator%s", var.environment == "dev" ? "-dev" : "")

  certificate_arn = "arn:aws:acm:us-east-1:111111111111:certificate/xxx"
}

resource "aws_cloudfront_distribution" "website" {
  origin {
    domain_name = aws_s3_bucket.static_website.website_endpoint
    origin_path = ""
    origin_id   = local.s3_origin_id

    custom_origin_config {
      http_port              = 80
      https_port             = 443
      origin_protocol_policy = "http-only"
      origin_ssl_protocols   = ["TLSv1.2"]
    }

    custom_header {
      name  = "User-Agent"
      value = var.secret
    }
  }

  web_acl_id          = aws_wafv2_web_acl.cdn.arn
  comment             = format("CDN for %s.xxx.report S3 Bucket", var.environment == "dev" ? "dev" : "www")
  enabled             = true
  is_ipv6_enabled     = true
  default_root_object = "index.html"
  aliases             = var.environment == "dev" ? ["dev.xxx.yyy"] : ["www.xxx.report", "xxx.yyy"]

  custom_error_response {
    error_code         = 403
    response_page_path = "/index.html"
    response_code      = 404
  }

  custom_error_response {
    error_code         = 404
    response_page_path = "/index.html"
    response_code      = 404
  }

  default_cache_behavior {
    target_origin_id = local.s3_origin_id
    allowed_methods  = ["GET", "HEAD"]
    cached_methods   = ["GET", "HEAD"]

    forwarded_values {
      query_string = false

      cookies {
        forward = "none"
      }
    }

    lambda_function_association {
      event_type   = "origin-response"
      include_body = false
      lambda_arn   = "${data.aws_lambda_function.http_headers.arn}:${data.aws_lambda_function.http_headers.version}"
    }

    viewer_protocol_policy = "redirect-to-https"
  }

  restrictions {
    geo_restriction {
      restriction_type = "none"
    }
  }

  viewer_certificate {
    cloudfront_default_certificate = false
    acm_certificate_arn            = local.certificate_arn
    minimum_protocol_version       = "TLSv1.2_2018"
    ssl_support_method             = "sni-only"
  }

  tags = merge(
    local.common_tags,
    map(
      "Name", format("CDN for %s.xxx.yyy S3 Bucket", var.environment == "dev" ? "dev" : "www")
    )
  )
}

resource "aws_cloudfront_distribution" "aggregator" {
  origin {
    domain_name = var.aggregator_gateway
    origin_path = "/${var.environment}"
    origin_id   = local.aggregator_origin_id

    custom_origin_config {
      http_port              = 80
      https_port             = 443
      origin_protocol_policy = "https-only"
      origin_ssl_protocols   = ["TLSv1.2"]
    }

    custom_header {
      name  = "X-RefererSalt"
      value = var.api_gateway_salt
    }
  }

  web_acl_id      = aws_wafv2_web_acl.cdn.arn
  comment         = format("CDN for aggregator%s.xxx.yyy API GW", var.environment == "dev" ? "-dev" : "")
  enabled         = true
  is_ipv6_enabled = true
  aliases         = [format("aggregator%s.xxx.yyy", var.environment == "dev" ? "-dev" : "")]

  default_cache_behavior {
    target_origin_id = local.aggregator_origin_id
    allowed_methods  = ["GET", "HEAD", "DELETE", "OPTIONS", "PATCH", "POST", "PUT"]
    cached_methods   = ["GET", "HEAD"]

    default_ttl = var.cdn_ttl
    max_ttl     = var.cdn_max_ttl

    forwarded_values {
      query_string = true
      headers = [
        "Accept-Encoding",
        "Accept-Language",
        "Authorization"
      ]

      cookies {
        forward = "none"
      }
    }

    lambda_function_association {
      event_type   = "origin-response"
      include_body = false
      lambda_arn   = "${data.aws_lambda_function.http_headers.arn}:${data.aws_lambda_function.http_headers.version}"
    }

    viewer_protocol_policy = "redirect-to-https"
  }

  restrictions {
    geo_restriction {
      restriction_type = "none"
    }
  }

  viewer_certificate {
    cloudfront_default_certificate = false
    acm_certificate_arn            = local.certificate_arn
    minimum_protocol_version       = "TLSv1.2_2018"
    ssl_support_method             = "sni-only"
  }

  tags = merge(
    local.common_tags,
    map(
      "Name", format("CDN for aggregator%s.xxx.yyy API GW", var.environment == "dev" ? "-dev" : "")
    )
  )
}
