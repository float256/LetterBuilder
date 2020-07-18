#====== Script destination
# start pre-update database

$updateScriptDir = Split-Path $MyInvocation.MyCommand.Path -Parent
. "$updateScriptDir\_db_migration.inc.ps1"

#start here
ExecDbUpdate "LetterBuilderWebAdmin" "pre";

