# 1. Read the version from build.txt
$content = Get-Content "build.txt"
# Regex to find "version = 0.x.x"
$versionMatch = $content | Select-String "version\s*=\s*([0-9.]+)"

if ($versionMatch) {
    $version = $versionMatch.Matches.Groups[1].Value
    $tagName = "v$version"

    Write-Host "Found build.txt version: $version"

    # 2. Check if this tag already exists locally or remotely
    $existingTags = git tag -l $tagName
    
    if ($existingTags) {
        Write-Host "Tag $tagName already exists. Skipping auto-publish."
    }
    else {
        Write-Host "New version detected! Preparing release..."

        # 3. Stage and Commit everything (so the tag captures your latest work)
        git add .
        git commit -m "Auto-release version $version"

        # 4. Create the Tag
        git tag $tagName
        Write-Host "Created tag: $tagName"

        # 5. Push to GitHub
        git push origin master
        git push origin $tagName
        
        Write-Host "SUCCESS: v$version has been pushed to GitHub!"
    }
}
else {
    Write-Host "Could not find version number in build.txt"
}