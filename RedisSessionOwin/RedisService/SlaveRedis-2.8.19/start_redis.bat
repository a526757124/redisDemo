@echo off

rem 当前bat的作用
echo ==================begin========================

cls 

SET Redis_PATH=%~d0

SET Redis_DIR=%~dp0

%color 0a% 

TITLE Redis 管理程序 Power By 焰尾迭 (http://yanweidie.cnblogs.com)

CLS 

ECHO. 

ECHO. * * Redis 管理程序 Power By 焰尾迭 (http://yanweidie.cnblogs.com)  *  

ECHO. 

:MENU 

ECHO. * Redis 进程list *  

tasklist|findstr /i "redis-server.exe"

ECHO. 

    ECHO.  [1] 启动Redis  

    ECHO.  [2] 关闭Redis  

    ECHO.  [3] 重启Redis 

    ECHO.  [4] 退 出 

ECHO. 

 
ECHO.请输入选择项目的序号:

set /p ID=

    IF "%id%"=="1" GOTO start 

    IF "%id%"=="2" GOTO stop 

    IF "%id%"=="3" GOTO restart 

    IF "%id%"=="4" EXIT

PAUSE 

 

:start 

    call :startRedis

    GOTO MENU

 

:stop 

    call :shutdownRedis

    GOTO MENU

 

:restart 

    call :shutdownRedis

    call :startRedis

    GOTO MENU

 

:shutdownRedis

    ECHO. 

    ECHO.关闭Redis...... 

    taskkill /F /IM redis-server.exe > nul

    ECHO.OK,关闭所有Redis 进程

    goto :eof

 

:startRedis

    ECHO. 

    ECHO.启动Redis...... 

    IF NOT EXIST "%Redis_DIR%redis-server.exe" ECHO "%Redis_DIR%redis-server.exe"不存在 

 

    %Redis_PATH% 

 

    cd "%Redis_DIR%" 

 

    IF EXIST "%Redis_DIR%redis-server.exe" (

        echo "start '' redis-server.exe"

        start /b redis-server.exe redis.conf

    )

    ECHO.OK

    goto :eof