
ALTER TABLE mp_Sites ADD COLUMN LdapUserDNKey VARCHAR(10);
UPDATE mp_Sites SET LdapUserDNKey = 'uid';
ALTER TABLE mp_Sites ALTER COLUMN LdapUserDNKey SET DEFAULT 'uid';



