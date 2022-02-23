SET SrcDir="..\..\..\iTSLibrary\iTSLibrary\bin\Debug\"
SET DstDir=".\iTSLibrary\"

SET DirList="Image" "zh-TW"
SET FileList=AsyncSocket.dll AsyncSocket.pdb Geometry.dll Geometry.pdb GLCore.dll GLCore.pdb GLStyle.dll GLStyle.pdb GLUI.dll GLUI.pdb Hasp.dll Hasp.pdb hasp_net_windows.dll IniFiles.dll IniFiles.pdb LogManager.dll LogManager.pdb MapReader.dll MapReader.pdb MD5Hash.dll MD5Hash.pdb SerialData.dll SerialData.pdb Serialization.dll Serialization.pdb Style.ini ThreadSafety.dll ThreadSafety.pdb WaitTask.dll WaitTask.pdb

FOR %%i IN (%DirList%) DO (
	IF EXIST "%DstDir%%%i" RD /Q /S "%DstDir%%%i"
)

FOR %%i IN (%FileList%) DO (
	IF EXIST "%DstDir%%%i" DEL "%DstDir%%%i"
)

FOR %%i IN (%DirList%) DO (
	XCOPY "%SrcDir%%%i" "%DstDir%%%i" /I /S /E
)

FOR %%i IN (%FileList%) DO (
	XCOPY "%SrcDir%%%i" "%DstDir%%%i"*
)

PAUSE
