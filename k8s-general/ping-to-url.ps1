# Check if system is ready by request status URL

$systemStatusUrl = 'http://52.137.31.128/'    
$sleepBetweenRetriesInSec = 1 

[Net.ServicePointManager]::SecurityProtocol = [Net.SecurityProtocolType]::Tls12

Write-Host "Check system status by request $systemStatusUrl"

$retries = 0
while ($true) {
    $statusCode = 0
    #$message = ""
    try {
        $response = Invoke-WebRequest -URI $systemStatusUrl -Method 'GET'
        $statusCode = [int]$response.StatusCode
    }
    catch {
        #[System.Net.WebException]
        If ($_.Exception.Response.StatusCode.value__) {
            $statusCode = $_.Exception.Response.StatusCode.value__
        }
        # If ($_.Exception.Message) {
        #     $message = ($_.Exception.Message).ToString().Trim();
        # }
    }

    Write-Host "  [$retries] Response status code: $statusCode"
   
    $retries++
    Start-Sleep -Seconds $sleepBetweenRetriesInSec;
}