@echo off
SET ENV=%1

IF '%ENV%'=='' (GOTO usage)

ECHO --------------------------------
ECHO   Updating PmsStorage DB
ECHO --------------------------------
call db_migration %ENV% update PmsStorage

ECHO --------------------------------
ECHO   Updating PmsStorageRawReservations DB
ECHO --------------------------------
call db_migration %ENV% update PmsStorageRawReservations

ECHO --------------------------------
ECHO   Updating PmsStorageSync DB
ECHO --------------------------------
call db_migration %ENV% update PmsStorageSync

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