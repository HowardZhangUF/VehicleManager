SET SrcDir="..\..\..\iTSLibrary\iTSLibrary\bin\Debug\"
SET DstDir=".\iTSLibrary\"

SET DirList=
SET FileList=AsyncSocket.dll AsyncSocket.pdb LogManager.dll LogManager.pdb SerialData.dll SerialData.pdb Serialization.dll Serialization.pdb WaitTask.dll WaitTask.pdb

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
