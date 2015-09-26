function ensureSavedEmailTable(db) 
{
  
  db.run('create table if not exists mp_emailmessages (' +
               'messageid varchar(255) not null primary key,' +
               'acountguid char(36) not null,' +
               'fromname varchar(100),' +
               'fromaddress varchar(100),' +
               'toaddress varchar(100),' +
               'subject varchar(255),' +
               'body text,' +
               'hashtmlbody integer not null,' +
               'messagedate varchar(100),' +
               'ccaddress varchar(255))');
  
 
}


function saveMessage(db, messageid, acountguid,fromname,fromaddress,toaddress,subject,body,hashtmlbody,messagedate,ccaddress) 
{
  
  ensureSavedEmailTable(db);
  
  db.run('insert into mp_emailmessages (messageid, acountguid,fromname,fromaddress,toaddress,subject,body,hashtmlbody,messagedate,ccaddress) values (?,?,?,?,?,?,?,?,?,?)', [messageid, acountguid,fromname,fromaddress,toaddress,subject,body,hashtmlbody,messagedate,ccaddress]);
  
}

function messageExists(db, messageid) 
{
  
  ensureSavedQueryTable(db);
  
  var rs = db.run('select subject from mp_emailmessages where messageid = ?',[messageid]);
  	
  if(rs)return true;
  
  return false;
  	
  
}

function deleteMessage(db, messageid) 
{
  
  ensureSavedQueryTable(db) ;
 
  db.run('delete from mp_emailmessages where messageid = ?', [messageid]);
  
}

function getMessage(db, messageid) 
{

    var rs = db.run('select * from mp_emailmessages where messageid = ?',[messageid]);
    return db.resultSetToObject(rs);  

}

function getMessageHeaders(db, acountguid, pageNumber, pageSize) 
{
    var offSet = pageNumber * pageSize;
    
    var query = 'select messageid, acountguid,fromname,fromaddress,toaddress,subject,hashtmlbody,messagedate,ccaddress from mp_emailmessages whrere acountguid = ? limit ' + pageSize ;
    	
    if(pageNumber > 1) query += ' offset ' + (pageNumber * pageSize) ;
    
    //query += + ';';
    	
    return db.run(query,[acountguid]);
    

}
