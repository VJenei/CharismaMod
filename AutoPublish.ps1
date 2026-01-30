Set-Location $PSScriptRoot

$content = Get-Content "build.txt"
$versionMatch = $content | Select-String "version\s*=\s*([0-9.]+)"

if ($versionMatch) {
    $version = $versionMatch.Matches.Groups[1].Value
    $tagName = "v$version"

    Write-Host "Found build.txt version: $version"

    $existingTags = git tag -l $tagName
    
    if ($existingTags) {
        Write-Host "Tag $tagName already exists. Skipping auto-publish."
    }
    else {
        Write-Host "New version detected! Preparing release..."

        git add .
        git commit -m "Auto-release version $version"

        git tag $tagName
        Write-Host "Created tag: $tagName"

        git push origin master
        git push origin $tagName
        
        Write-Host "SUCCESS: v$version has been pushed to GitHub!"
    }
}
else {
    Write-Host "Could not find version number in build.txt"
}