resource "aws_s3_bucket" "config" {
  count = var.environment == "dev" ? 0 : 1
  bucket = "xxx-config"

  tags = merge(
    local.common_tags,
    map(
      "Name", "AWS Config"
    )
  )
}

resource "aws_s3_bucket" "static_website" {
  bucket = format("%s-xxx-yyy", var.environment == "dev" ? "dev" : "www")

  website {
    index_document = "index.html"
    error_document = "index.html"

    routing_rules = ""
  }

  tags = merge(
    local.common_tags,
    map(
      "Name", format("%s.xxx.yyy static website", var.environment == "dev" ? "dev" : "www")
    )
  )
}

data "aws_iam_policy_document" "static_website_read_with_secret" {
  statement {
    sid       = "1"
    actions   = ["s3:GetObject"]
    resources = ["${aws_s3_bucket.static_website.arn}/*"]

    principals {
      type        = "AWS"
      identifiers = ["*"]
    }

    condition {
      test     = "StringEquals"
      variable = "aws:UserAgent"
      values   = [var.secret]
    }
  }
}

resource "aws_s3_bucket_policy" "static_website_read_with_secret" {
  bucket = aws_s3_bucket.static_website.id
  policy = data.aws_iam_policy_document.static_website_read_with_secret.json
}


resource "aws_s3_bucket" "cloudtrail" {
  count         = var.environment == "dev" ? 0 : 1
  bucket        = "xxx-app-cloudtrail"
  force_destroy = true

  tags = merge(
    local.common_tags,
    map(
      "Name", "CloudTrail bucket for log events"
    )
  )
}

data "aws_iam_policy_document" "cloudtrail" {
  count = var.environment == "dev" ? 0 : 1
  statement {
    sid       = "AWSCloudTrailAclCheck"
    actions   = ["s3:GetBucketAcl"]
    resources = [aws_s3_bucket.cloudtrail[0].arn]

    principals {
      type        = "Service"
      identifiers = ["cloudtrail.amazonaws.com"]
    }
  }

  statement {
    sid       = "AWSCloudTrailWrite"
    actions   = ["s3:PutObject"]
    resources = ["${aws_s3_bucket.cloudtrail[0].arn}/AWSLogs/${data.aws_caller_identity.current.account_id}/*"]

    principals {
      type        = "Service"
      identifiers = ["cloudtrail.amazonaws.com"]
    }

    condition {
      test     = "StringEquals"
      variable = "s3:x-amz-acl"
      values   = ["bucket-owner-full-control"]
    }
  }
}

resource "aws_s3_bucket_policy" "cloudtrail" {
  count  = var.environment == "dev" ? 0 : 1
  bucket = aws_s3_bucket.cloudtrail[0].id
  policy = data.aws_iam_policy_document.cloudtrail[0].json
}
