SET SrcDir="..\..\..\cs_pathfinding_advanced\Module.Pathfinding\bin\Debug\"
SET DstDir=".\Pathfinding\"

SET DirList=
SET FileList=Module.Map.dll Module.Map.pdb Module.Pathfinding.dll Module.Pathfinding.pdb

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
