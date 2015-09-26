DROP TABLE mp_UserProperties;

CREATE TABLE `mp_UserProperties` (
 `PropertyID` varchar(36) NOT NULL, 
 `UserGuid` varchar(36) NOT NULL,
 `PropertyName` varchar(255) NULL,
 `PropertyValueString` text NULL,
 `PropertyValueBinary` LongBlob NULL,
 `LastUpdatedDate` datetime NOT NULL,
 `IsLazyLoaded` bit NOT NULL,
 PRIMARY KEY (`PropertyID`)   
) ENGINE=MyISAM ;


ALTER TABLE mp_Pages ADD COLUMN `PageTitle` varchar(255) NULL;
ALTER TABLE mp_Pages ADD COLUMN AllowBrowserCache bit;
UPDATE mp_Pages SET AllowBrowserCache = 1;


ALTER TABLE mp_ModuleDefinitions ADD COLUMN DefaultCacheTime integer DEFAULT 0;
UPDATE mp_ModuleDefinitions SET DefaultCacheTime = 0;


