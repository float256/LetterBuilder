$scriptDir = Split-Path $MyInvocation.MyCommand.Path -Parent
$basePath = "$scriptDir\.."
$migrationTool = "$basePath\Tools\DBMigration\DBMigration.exe"

$dbSettings = @{
    "LetterBuilderWebAdmin" = @{
        "connStrName"   = "default";
        "dir"           = "db-migrations";
        "path"          = "sf-package\LetterBuilderWebAdminPkg\Code";
        "localPath"     = "LetterBuilderWebApi";
    };
};

function GetUtilParams($settingsName) {
    $connStrName    = $dbSettings[$settingsName].connStrName;
    $dir            = $dbSettings[$settingsName].dir;
    $path           = $dbSettings[$settingsName].path;

    $file = Get-ChildItem -Path ($basePath + "\" + $path) | where-object { $_.Name -eq "appsettings.json"}
    if ( -not $file ) {
        $file = Get-ChildItem -Path ($basePath + "\" + $path) | where-object { $_.Name -eq "sharedSettings.json"}
    }
    $file = $basePath + "\" + $path + "\" + $file
    if ( -not $file ) {
        throw [System.ArgumentException] "No configuration file found for $settingsName";
    }

    $reader = New-Object IO.StreamReader(Resolve-Path -Path $file)

    $connStr =  ($reader.ReadToEnd() | ConvertFrom-Json).ConnectionStrings.$connStrName

    $reader.Close()
    # $zip.Dispose()

    if( [string]::IsNullOrEmpty($connStr) ) {
        throw [System.ArgumentException] "No connection string found for $settingsName";
    }
    
    return @{
        "connStr" = "$connStr";
        "dir"     = "$basePath\$dir";
    };
}

function ExecDbUpdate($settingsName, $prePost) {
    $utilParams = GetUtilParams $settingsName;

    $connStr = $utilParams["connStr"];
    $dir     = $utilParams["dir"];

    $updateCommand = "$migrationTool update --$prePost --conn=""$connStr"" --dir=$dir || exit 1";

    cmd.exe /c $updateCommand
    if ($LASTEXITCODE -ne "0") {
        throw $LASTEXITCODE
    }
}

function ExecMigrationInfo($settingsName) {
    $utilParams = GetUtilParams $settingsName;

    $connStr = $utilParams["connStr"];
    $dir     = $utilParams["dir"];

    $updateCommand = "$migrationTool info --conn=""$connStr"" --dir=$dir || exit 1";

    cmd.exe /c $updateCommand
    if ($LASTEXITCODE -ne "0") {
        throw $LASTEXITCODE
    }
}

function ExecMark($settingsName, $file) {
    $utilParams = GetUtilParams $settingsName;

    $connStr = $utilParams["connStr"];
    $dir     = $utilParams["dir"];

    $updateCommand = "$migrationTool mark --conn=""$connStr"" --dir=$dir $file || exit 1";

    cmd.exe /c $updateCommand
    if ($LASTEXITCODE -ne "0") {
        throw $LASTEXITCODE
    }
}