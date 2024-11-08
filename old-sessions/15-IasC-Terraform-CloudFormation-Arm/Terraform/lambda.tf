data "aws_lambda_function" "http_headers" {
  provider      = aws.virginia
  function_name = "xxx-app-headers"
  qualifier     = "latest"
}
