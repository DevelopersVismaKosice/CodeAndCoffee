#.DistributionList.Items[] | select(.DomainName == "zzz.cloudfront.net") | .Id
data "aws_route53_zone" "this" {
  zone_id      = "ZZZ"
  private_zone = false
}

resource "aws_route53_record" "auth_cognito" {
  name    = aws_cognito_user_pool_domain.this.domain
  type    = "A"
  zone_id = data.aws_route53_zone.this.zone_id

  alias {
    evaluate_target_health = false
    name                   = aws_cognito_user_pool_domain.this.cloudfront_distribution_arn
    # This zone_id is fixed
    zone_id = "zzz"
  }
}

resource "aws_route53_record" "apex" {
  count   = var.environment == "dev" ? 0 : 1
  zone_id = data.aws_route53_zone.this.zone_id
  name    = "xxx.yyy"
  type    = "A"

  alias {
    evaluate_target_health = false
    name                   = aws_cloudfront_distribution.website.domain_name
    zone_id                = aws_cloudfront_distribution.website.hosted_zone_id
  }
}

resource "aws_route53_record" "website" {
  zone_id = data.aws_route53_zone.this.zone_id
  name    = format("%s.xxx.yyy", var.environment == "dev" ? "dev" : "www")
  type    = "CNAME"
  ttl     = "60"
  records = [aws_cloudfront_distribution.website.domain_name]
}

resource "aws_route53_record" "aggregator" {
  zone_id = data.aws_route53_zone.this.zone_id
  name    = format("aggregator%s.xxx.yyy", var.environment == "dev" ? "-dev" : "")
  type    = "CNAME"
  ttl     = "60"
  records = [aws_cloudfront_distribution.aggregator.domain_name]
}
