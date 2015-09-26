function ensureSavedQueryTable(db) 
{
  
  db.run('create table if not exists savedqueries (' +
               'id integer not null primary key autoincrement,' +
               'name varchar(255),' +
               'query text)');
  
 
}


function createSavedQuery(db, queryName, queryText) 
{
  
  ensureSavedQueryTable(db);
  
  db.run('insert into savedqueries (name, query) values (?, ?)', [queryName, queryText]);
  
}

function updateSavedQuery(db, id, queryName, queryText) 
{
  
  ensureSavedQueryTable(db);
  
  db.run('update savedqueries set name = ?, query = ? where id = ?',
  	[queryName, queryText, id]);
  
}

function deleteSavedQuery(db, queryID) 
{
  
  ensureSavedQueryTable(db) ;
  var query = 'delete from savedqueries where id = ' + queryID + ';';
  
  db.run(query);
  
}
