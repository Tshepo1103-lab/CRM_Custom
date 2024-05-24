
param (
    [string]$sonarProjectKey,
    [string]$sonarHostUrl,
    [string]$sonarToken
)

# Function to poll for SonarCloud Quality Gate status
function Get-SonarQualityGateStatus {
    param (
        [string]$projectKey,
        [string]$token,
        [string]$sonarHost        
    )
    
    # $authHeader = @{ Authorization = "Basic " + [Convert]::ToBase64String([Text.Encoding]::ASCII.GetBytes(":$token")) }
    $authHeader = New-Object "System.Collections.Generic.Dictionary[[String],[String]]"
    $authHeader.Add("Authorization", "Bearer $token")
    $qualityGateStatusUrl = "$sonarHost/api/qualitygates/project_status?projectKey=$projectKey"

    do {
        Start-Sleep -Seconds 10  # Polling interval
        # $response = Invoke-RestMethod -Uri $qualityGateStatusUrl -Method Get -Headers $authHeader
        $response = Invoke-RestMethod $qualityGateStatusUrl -Method 'GET' -Headers $authHeader
        $status = $response.projectStatus.status
        Write-Output "Current Quality Gate Status: $status"
    }
    while ($status -eq "NONE" -or $status -eq "OK")
    
    return $status
}

# Invoke the function
$status = Get-SonarQualityGateStatus -projectKey $sonarProjectKey -token $sonarToken -sonarHost $sonarHostUrl

# Check status and act accordingly
if ($status -ne "OK") {
    Write-Output "Quality Gate failed please check dashboard for more information: https://sonarcloud.io/project/overview?id=$sonarProjectKey"
    exit 1  # Exit with error to fail the build in CI
} else {
    Write-Output "Quality Gate passed"
}
