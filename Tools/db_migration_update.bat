@echo off
SET ENV=%1

IF '%ENV%'=='' (GOTO usage)

ECHO --------------------------------
ECHO   Updating LetterBuilderWebAdmin DB
ECHO --------------------------------
call db_migration %ENV% update LetterBuilderWebAdmin

pause

GOTO end

:usage
    ECHO --------------------------------
    ECHO   TravelLine DB migration tool
    ECHO --------------------------------
    ECHO Usage: %SELF_NAME% ^<env^>
    EXIT /B 1
GOTO end

:end
