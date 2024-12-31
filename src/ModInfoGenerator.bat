setlocal

for /f "delims=-" %%v in ('git describe --tags --always 2^>nul') do set GIT_VERSION=%%v

if not defined GIT_VERSION (
    echo Could not get git version.
    exit /b 1
)

(
echo internal static class ModInfo
echo {
echo     internal const string
echo     ModGUID = "Becko.KogamaTools",
echo     ModName = "KogamaTools",
echo     ModVersion = "%GIT_VERSION%";
echo }
) > ModInfo.cs

endlocal
