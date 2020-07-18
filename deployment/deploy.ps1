#====== Script destination
# creating deployment package with msbuild package task and copying
# db-migration scripts to package directory $packageDstDir

param(  
        [Parameter(Mandatory=$True)]
        [String]
        $Version
     )

function WriteVersionToServiceManifest($serviceManifestFile, $newVersion) {
    [xml]$xml = (Get-Content $serviceManifestFile)
    $xml.ServiceManifest.Version = $newVersion
    $xml.ServiceManifest.CodePackage.Version = $newVersion
    $xml.ServiceManifest.ConfigPackage.Version = $newVersion
    $xml.Save($serviceManifestFile)
}

function WriteVersionToApplicationManifest($applicationManifestFile, $newVersion) {
    [xml]$xml = (Get-Content $applicationManifestFile)
    $xml.ApplicationManifest.ApplicationTypeVersion =$Version
    
    WriteVersionToServiceManifestImport -xml $xml -serviceManifestName "LetterBuilderFrontendPkg" -newVersion $newVersion
    WriteVersionToServiceManifestImport -xml $xml -serviceManifestName "LetterBuilderWebApiPkg" -newVersion $newVersion
    WriteVersionToServiceManifestImport -xml $xml -serviceManifestName "LetterBuilderWebAdminPkg" -newVersion $newVersion

    $xml.Save($applicationManifestFile)
}

function WriteVersionToServiceManifestImport($xml, $serviceManifestName, $newVersion) {
    $node = $xml.ApplicationManifest.ServiceManifestImport | where {$_.ServiceManifestRef.ServiceManifestName -eq $serviceManifestName}
    $node.ServiceManifestRef.ServiceManifestVersion = $newVersion
}

$baseDir = Split-Path (Split-Path $MyInvocation.MyCommand.Path -Parent) -Parent

$applicationManifestFile = "$baseDir/sf-package/ApplicationManifest.xml"

$letterBuilderFrontendServiceManifestFile = "$baseDir/sf-package/LetterBuilderFrontendPkg/ServiceManifest.xml"
$letterBuilderWebApiServiceManifestFile = "$baseDir/sf-package/LetterBuilderWebApiPkg/ServiceManifest.xml"
$letterBuilderWebAdminServiceManifestFile = "$baseDir/sf-package/LetterBuilderWebAdminPkg/ServiceManifest.xml"

#start here
WriteVersionToApplicationManifest -applicationManifestFile $applicationManifestFile -newVersion $Version

WriteVersionToServiceManifest -serviceManifestFile $letterBuilderFrontendServiceManifestFile -newVersion $Version
WriteVersionToServiceManifest -serviceManifestFile $letterBuilderWebApiServiceManifestFile -newVersion $Version
WriteVersionToServiceManifest -serviceManifestFile $letterBuilderWebAdminServiceManifestFile -newVersion $Version