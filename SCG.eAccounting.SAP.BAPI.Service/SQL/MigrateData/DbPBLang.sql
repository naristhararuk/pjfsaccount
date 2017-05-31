INSERT INTO DbPBLang
(
	PBID,
	LanguageID,
	Description,
	Comment,
	Active,UpdBy,UpdDate,CreBy,CreDate,UpdPgm
)

(
	SELECT 
	(	
		SELECT 
			DbPB.PBID 
		FROM 
			DbPB 
		WHERE  
			IMPORT.dbo.dbo_MS_PB.PB_Code = DbPB.PBCode
	) AS PBLanga,

	2 AS LanguageID,
	'ENG-' + Name2 AS Description,
	'' AS Comment,
	
	'true' AS Active ,
	1 AS aa ,
	getdate() as date1,
	1 AS aa,
	getdate() as date2,
	'KLA' AS UpdPgm

	FROM IMPORT.dbo.dbo_MS_PB
)