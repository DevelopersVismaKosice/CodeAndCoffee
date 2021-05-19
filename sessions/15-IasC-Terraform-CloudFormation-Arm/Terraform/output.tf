output "headers_lambda" {
  value = "${data.aws_lambda_function.http_headers.arn}:${data.aws_lambda_function.http_headers.version}"
}
