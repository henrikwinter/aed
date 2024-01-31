
var SchemaRoots_datafields = [
 { name: 'id', type: 'decimal' },
 { name: 'parentid', type: 'decimal' },
 { name: 'value', type: 'string' },
 { name: 'Label', type: 'string' },
 { name: 'Table', type: 'string' }
];
var Users_datafields = [
 { name: 'Id', type: 'string' },
 { name: 'Email', type: 'string' },
 { name: 'Emailconfirmed', type: 'decimal' },
 { name: 'Passwordhash', type: 'string' },
 { name: 'Securitystamp', type: 'string' },
 { name: 'Phonenumber', type: 'string' },
 { name: 'Phonenumberconfirmed', type: 'decimal' },
 { name: 'Twofactorenabled', type: 'decimal' },
 { name: 'Lockoutenddateutc', type: 'date' },
 { name: 'Lockoutenabled', type: 'decimal' },
 { name: 'Accessfailedcount', type: 'decimal' },
 { name: 'Username', type: 'string' }
];
var Users_columns = [
 { text: '§Id§', datafield: 'Id' },
 { text: '§Email§', datafield: 'Email' },
 { text: '§Emailconfirmed§', datafield: 'Emailconfirmed' },
 { text: '§Passwordhash§', datafield: 'Passwordhash' },
 { text: '§Securitystamp§', datafield: 'Securitystamp' },
 { text: '§Phonenumber§', datafield: 'Phonenumber' },
 { text: '§Phonenumberconfirmed§', datafield: 'Phonenumberconfirmed' },
 { text: '§Twofactorenabled§', datafield: 'Twofactorenabled' },
 { text: '§Lockoutenddateutc§', datafield: 'Lockoutenddateutc' },
 { text: '§Lockoutenabled§', datafield: 'Lockoutenabled' },
 { text: '§Accessfailedcount§', datafield: 'Accessfailedcount' },
 { text: '§Username§', datafield: 'Username' }
];

var Flows_datafields = [
 { name: 'Id_Flow', type: 'decimal' },
 { name: 'Id_Parrentflow', type: 'decimal' },
 { name: 'Bid_Flow', type: 'string' },
 { name: 'Flowname', type: 'string' },
 { name: 'Stepname', type: 'string' },
 { name: 'Controller', type: 'string' },
 { name: 'Action', type: 'string' },
 { name: 'Title', type: 'string' },
 { name: 'Flowhistory', type: 'string' },
 { name: 'Pvariables', type: 'string' },
 { name: 'Recordvalidfrom', type: 'date' },
 { name: 'Recordvalidto', type: 'date' },
 { name: 'Datavalidfrom', type: 'date' },
 { name: 'Datavalidto', type: 'date' },
 { name: 'Status', type: 'string' },
 { name: 'Creator', type: 'string' },
 { name: 'Modifiers', type: 'string' },
 { name: 'Orggroup', type: 'decimal' },
 { name: 'Attributum', type: 'string' },
 { name: 'Property', type: 'string' },
 { name: 'Assignment', type: 'string' }
];
var Flows_columns = [
 //{ text: '§Id_Flow§', datafield: 'Id_Flow', width: 30 },
 //{ text: '§Id_Parrentflow§', datafield: 'Id_Parrentflow' },
 { text: '§Bid_Flow§', datafield: 'Bid_Flow', columntype: 'textbox', filtertype: 'textbox', width: 100 },
 { text: '§Title§', datafield: 'Title' },
 { text: '§Flowname§', datafield: 'Flowname', width: 120 },
 { text: '§Stepname§', datafield: 'Stepname', width: 120 },
 //{ text: '§Controller§', datafield: 'Controller' },
 //{ text: '§Action§', datafield: 'Action' },
  //{ text: '§Flowhistory§', datafield: 'Flowhistory' },
 //{ text: '§Pvariables§', datafield: 'Pvariables' },
 { text: '§Recordvalidfrom§', datafield: 'Recordvalidfrom', cellsalign: 'right', width: 130 },
 //{ text: '§Recordvalidto§', datafield: 'Recordvalidto' },
 //{ text: '§Datavalidfrom§', datafield: 'Datavalidfrom' },
 //{ text: '§Datavalidto§', datafield: 'Datavalidto' },
 //{ text: '§Status§', datafield: 'Status' },
 //{ text: '§Creator§', datafield: 'Creator' },
 //{ text: '§Modifiers§', datafield: 'Modifiers' },
 //{ text: '§Orggroup§', datafield: 'Orggroup' },
 { text: '§Attributum§', datafield: 'Attributum', width: 100 }
 //{ text: '§Property§', datafield: 'Property' },
 //{ text: '§Assignment§', datafield: 'Assignment' }
];

var Organization_datafields_forTree = [
 { name: 'Id_Organization', type: 'decimal' },
 { name: 'Id_Parent', type: 'decimal' },
{ name: 'Id_Scopeofactivity', type: 'decimal' },
 { name: 'Id_Ord', type: 'decimal' },
 { name: 'Bid_Organization', type: 'string' },
 { name: 'Name', type: 'string' },
 { name: 'Title', type: 'string' },
 { name: 'Shortname', type: 'string' },
 //{ name: 'Xmldata', type: 'string' },
 { name: 'Recordtype', type: 'string' },
 //{ name: 'Id_Flows', type: 'decimal' },
 //{ name: 'Cpyid', type: 'decimal' },
 //{ name: 'Cpyparrent', type: 'decimal' },
 //{ name: 'Recordvalidfrom', type: 'date' },
 //{ name: 'Recordvalidto', type: 'date' },
 //{ name: 'Datavalidfrom', type: 'date' },
 { name: 'Datavalidto', type: 'date' },
 { name: 'Status', type: 'string' },
 { name: 'Orgtype', type: 'string' },
 { name: 'Tmpid_Persons', type: 'decimal' },
 //{ name: 'Creator', type: 'string' },
 //{ name: 'Modifiers', type: 'string' },
 //{ name: 'Orggroup', type: 'decimal' },
 { name: 'Attributum', type: 'string' }
 //{ name: 'Property', type: 'string' },
 //{ name: 'Assignment', type: 'string' }
]

var Organization_datafields = [
 { name: 'Id_Organization', type: 'decimal' },
 { name: 'Id_Parent', type: 'decimal' },
{ name: 'Id_Scopeofactivity', type: 'decimal' },
 { name: 'Id_Ord', type: 'decimal' },
 { name: 'Bid_Organization', type: 'string' },
 { name: 'Name', type: 'string' },
 { name: 'Title', type: 'string' },
 { name: 'Shortname', type: 'string' },
 { name: 'Xmldata', type: 'string' },
 { name: 'Recordtype', type: 'string' },
 { name: 'Id_Flows', type: 'decimal' },
 { name: 'Cpyid', type: 'decimal' },
 { name: 'Cpyparrent', type: 'decimal' },
 { name: 'Recordvalidfrom', type: 'date' },
 { name: 'Recordvalidto', type: 'date' },
 { name: 'Datavalidfrom', type: 'date' },
 { name: 'Datavalidto', type: 'date' },
 { name: 'Status', type: 'string' },
 { name: 'Creator', type: 'string' },
 { name: 'Modifiers', type: 'string' },
 { name: 'Orggroup', type: 'decimal' },
 { name: 'Attributum', type: 'string' },
 { name: 'Property', type: 'string' },
 { name: 'Assignment', type: 'string' },
 { name: 'Orgtype', type: 'string' }
];
var Organization_columns = [
 { text: '§Id_Organization§', datafield: 'Id_Organization', hidden: true },
 { text: '§Id_Parent§', datafield: 'Id_Parent', hidden: true },
 { text: '§Id_Scopeofactivity§', datafield: 'Id_Scopeofactivity', hidden: true },
 { text: '§Id_Ord§', datafield: 'Id_Ord', hidden: true },
 { text: '§Bid_Organization§', datafield: 'Bid_Organization' ,width:'10%' },
 { text: '§Name§', datafield: 'Name',width:'20%'  },
 { text: '§Title§', datafield: 'Title', width:'20%' },
 { text: '§Shortname§', datafield: 'Shortname', width: '20%' },
 { text: '§Xmldata§', datafield: 'Xmldata', hidden: true },
 { text: '§Recordtype§', datafield: 'Recordtype', width: '10%' },
 { text: '§Id_Flows§', datafield: 'Id_Flows', hidden: true },
 { text: '§Cpyid§', datafield: 'Cpyid', hidden: true },
 { text: '§Cpyparrent§', datafield: 'Cpyparrent', hidden: true },
 { text: '§Recordvalidfrom§', datafield: 'Recordvalidfrom', hidden: true },
 { text: '§Recordvalidto§', datafield: 'Recordvalidto', hidden: true },
 { text: '§Datavalidfrom§', datafield: 'Datavalidfrom' },
 { text: '§Datavalidto§', datafield: 'Datavalidto', hidden: true },
 { text: '§Status§', datafield: 'Status', hidden: true },
 { text: '§Creator§', datafield: 'Creator', hidden: true },
 { text: '§Modifiers§', datafield: 'Modifiers', hidden: true },
 { text: '§Orggroup§', datafield: 'Orggroup', hidden: true },
 { text: '§Attributum§', datafield: 'Attributum', hidden: true },
 { text: '§Property§', datafield: 'Property', hidden: true },
 { text: '§Assignment§', datafield: 'Assignment', hidden: true },
 { text: '§Orgtype§', datafield: 'Orgtype'}
]

var Checklistelement_datafields = [
 { name: 'Id_Checklistelement', type: 'Number' },
 { name: 'Id_Organization', type: 'Number' },
 { name: 'Id_Flows', type: 'Number' },
 { name: 'Cpyid', type: 'Number' },
 { name: 'Recordtype', type: 'Varchar' },
 { name: 'Itemtype', type: 'Varchar' },
 { name: 'Name', type: 'Varchar' },
 { name: 'Description', type: 'Varchar' },
 { name: 'Xmldata', type: 'Varchar' },
 { name: 'Recordvalidfrom', type: 'date' },
 { name: 'Recordvalidto', type: 'date' },
 { name: 'Datavalidfrom', type: 'date' },
 { name: 'Datavalidto', type: 'date' },
 { name: 'Status', type: 'Varchar' },
 { name: 'Creator', type: 'Varchar' },
 { name: 'Modifiers', type: 'Varchar' },
 { name: 'Orggroup', type: 'Number' },
 { name: 'Attributum', type: 'Varchar' },
 { name: 'Property', type: 'Varchar' },
 { name: 'Assignment', type: 'Varchar' }
]
var Checklistelement_columns = [
 { text: '§Id_Checklistelement§', datafield: 'Id_Checklistelement' ,hidden:true},
 { text: '§Id_Organization§', datafield: 'Id_Organization', hidden: true },
 { text: '§Id_Flows§', datafield: 'Id_Flows', hidden: true },
 //{ text: '§Cpyid§', datafield: 'Cpyid' },
 { text: '§Recordtype§', datafield: 'Recordtype', hidden: true },
 { text: '§Itemtype§', datafield: 'Itemtype', hidden: true },
 { text: '§Name§', datafield: 'Name',width:'60%' },
 { text: '§Description§', datafield: 'Description', hidden: true },
 //{ text: '§Xmldata§', datafield: 'Xmldata' },
 { text: '§Recordvalidfrom§', datafield: 'Recordvalidfrom', hidden: true },
 { text: '§Recordvalidto§', datafield: 'Recordvalidto', hidden: true },
 { text: '§Datavalidfrom§', datafield: 'Datavalidfrom' },
 { text: '§Datavalidto§', datafield: 'Datavalidto', hidden: true },
 { text: '§Status§', datafield: 'Status', hidden: true },
 { text: '§Creator§', datafield: 'Creator', hidden: true },
 { text: '§Modifiers§', datafield: 'Modifiers', hidden: true },
 { text: '§Orggroup§', datafield: 'Orggroup', hidden: true },
 { text: '§Attributum§', datafield: 'Attributum', hidden: true },
 { text: '§Property§', datafield: 'Property', hidden: true },
 { text: '§Assignment§', datafield: 'Assignment', hidden: true }
]

var Persons_datafields = [
 { name: 'Id_Persons', type: 'decimal' },
 { name: 'Cid_Person', type: 'string' },
 { name: 'Bid_Person', type: 'string' },
 { name: 'Usedname', type: 'string' },
 { name: 'Email', type: 'string' },
 { name: 'Birthfirstname', type: 'string' },
 { name: 'Birthlastname', type: 'string' },
 { name: 'Birthdate', type: 'date' },
 { name: 'Placeofbirth', type: 'string' },
 { name: 'Motherfirstname', type: 'string' },
 { name: 'Motherlastname', type: 'string' },
 { name: 'Xmldata', type: 'string' },
 { name: 'Id_Parent', type: 'decimal' },
 { name: 'Userid', type: 'string' }
]
var Persons_columns = [
 { text: '§Id_Persons§', datafield: 'Id_Persons' },
 { text: '§Cid_Person§', datafield: 'Cid_Person' },
 { text: '§Bid_Person§', datafield: 'Bid_Person' },
 { text: '§Usedname§', datafield: 'Usedname' },
 { text: '§Email§', datafield: 'Email' },
 { text: '§Birthfirstname§', datafield: 'Birthfirstname' },
 { text: '§Birthlastname§', datafield: 'Birthlastname' },
 { text: '§Birthdate§', datafield: 'Birthdate' },
 { text: '§Placeofbirth§', datafield: 'Placeofbirth' },
 { text: '§Motherfirstname§', datafield: 'Motherfirstname' },
 { text: '§Motherlastname§', datafield: 'Motherlastname' },
 { text: '§Xmldata§', datafield: 'Xmldata' },
 { text: '§Id_Parent§', datafield: 'Id_Parent' },
 { text: '§Userid§', datafield: 'Userid' }
]

var Vw_Users_datafields = [
 { name: 'Id', type: 'String' },
 { name: 'Id_Persons', type: 'Number' },
 { name: 'Username', type: 'string' },
 { name: 'Usedname', type: 'string' },
 { name: 'Email', type: 'string' }
]
var Vw_Users_columns = [
 { text: '§Id§', datafield: 'Id', hidden: true },
 { text: '§Id_Persons§', datafield: 'Id_Persons', width: 30 },
 { text: '§Username§', datafield: 'Username', width: 200 },
 { text: '§Usedname§', datafield: 'Usedname', width: 200 },
 { text: '§Email§', datafield: 'Email'},
]

var Dxxrefuserorggroup_datafields = [
 { name: 'Id', type: 'decimal' },
 { name: 'Username', type: 'string' },
 { name: 'Groupname', type: 'string' },
 { name: 'Flag', type: 'string' }
]
var Dxxrefuserorggroup_columns = [
 { text: '§Id§', datafield: 'Id' },
 { text: '§Username§', datafield: 'Username' },
 { text: '§Groupname§', datafield: 'Groupname' },
 { text: '§Flag§', datafield: 'Flag' }
]

var Dxxrefuserusergroup_datafields = [
 { name: 'Id', type: 'decimal' },
 { name: 'Username', type: 'string' },
 { name: 'Groupname', type: 'string' }
];
var Dxxrefuserusergroup_columns = [
 { text: '§Id§', datafield: 'Id' },
 { text: '§Username§', datafield: 'Username' },
 { text: '§Groupname§', datafield: 'Groupname' }
];

var Dxxrefusergrouprole_datafields = [
 { name: 'Id', type: 'string' },
 { name: 'Rolename', type: 'string' },
 { name: 'Groupname', type: 'string' }
]
var Dxxrefusergrouprole_columns = [
 { text: '§Id§', datafield: 'Id' },
 { text: '§Rolename§', datafield: 'Rolename' },
 { text: '§Groupname§', datafield: 'Groupname' }
]

var Dxusergroup_datafields = [
 { name: 'Id', type: 'decimal' },
 { name: 'Groupname', type: 'string' }
]
var Dxusergroup_columns = [
 { text: '§Id§', datafield: 'Id' },
 { text: '§Groupname§', datafield: 'Groupname' }
]

var Personpropertyesforview_datafields = [
 { name: 'Id_Prop', type: 'decimal' },
 { name: 'Table', type: 'string' },
 { name: 'Description', type: 'string' },
 { name: 'Recordtype', type: 'string' },
{ name: 'Datefrom', type: 'date' },
{ name: 'Dateto', type: 'date' },
]
var Personpropertyesforview_columns = [
 { text: '§Id_Prop§', datafield: 'Id_Prop', hidden: true, cellClassName: 'jqxgrd_customcel' },
 { text: '§Table§', datafield: 'Table', hidden: false, cellClassName: 'jqxgrd_customcel' },
 { text: '§Description§', datafield: 'Description', cellClassName: 'jqxgrd_customcel' },
 { text: '§Recordtype§', datafield: 'Recordtype', cellClassName: 'jqxgrd_customcel' },
 { text: '§Datefrom§', datafield: 'Datefrom', cellClassName: 'jqxgrd_customcel' },
 { text: '§Dateto§', datafield: 'Dateto', cellClassName: 'jqxgrd_customcel' }
]

var Lay_datafields = [
 { name: 'Id_Lay', type: 'decimal' },
 { name: 'Id_Persons', type: 'decimal' },
 { name: 'Cid_Persons', type: 'decimal' },
 { name: 'Id_Flows', type: 'decimal' },
 { name: 'Id_Agreement', type: 'decimal' },
 { name: 'Name', type: 'string' },
 { name: 'Root', type: 'string' },
 { name: 'Recordtype', type: 'string' },
 { name: 'Complextype', type: 'string' },
 { name: 'Xmldata', type: 'string' },
 { name: 'Laytype', type: 'string' },
 { name: 'Codefeor', type: 'string' },
 { name: 'Codeinternal', type: 'string' },
 { name: 'Laycategory', type: 'string' },
 { name: 'Datefrom', type: 'DateTime' },
 { name: 'Itemtype', type: 'string' },
 { name: 'Layrole', type: 'string' },
 { name: 'Layroot', type: 'string' }
];
var Lay_columns = [
 { text: '§Id_Lay§', datafield: 'Id_Lay' },
 { text: '§Id_Persons§', datafield: 'Id_Persons' },
 { text: '§Cid_Persons§', datafield: 'Cid_Persons' },
 { text: '§Id_Flows§', datafield: 'Id_Flows' },
 { text: '§Id_Agreement§', datafield: 'Id_Agreement' },
 { text: '§Name§', datafield: 'Name' },
 { text: '§Root§', datafield: 'Root' },
 { text: '§Recordtype§', datafield: 'Recordtype' },
 { text: '§Complextype§', datafield: 'Complextype' },
 { text: '§Xmldata§', datafield: 'Xmldata' },
 { text: '§Laytype§', datafield: 'Laytype' },
 { text: '§Codefeor§', datafield: 'Codefeor' },
 { text: '§Codeinternal§', datafield: 'Codeinternal' },
 { text: '§Laycategory§', datafield: 'Laycategory' },
 { text: '§Datefrom§', datafield: 'Datefrom' },
 { text: '§Itemtype§', datafield: 'Itemtype' },
 { text: '§Layrole§', datafield: 'Layrole' },
 { text: '§Layroot§', datafield: 'Layroot' }
];

var Jobs_datafields = [
 { name: 'Id_Jobs', type: 'decimal' },
 { name: 'Id_Parent', type: 'decimal' },
 { name: 'Id_Ord', type: 'decimal' },
 { name: 'Id_Flows', type: 'decimal' },
 { name: 'Name', type: 'string' },
 { name: 'Jobstype', type: 'string' },
 { name: 'Jobscategory', type: 'string' },
 { name: 'Recordtype', type: 'string' },
 { name: 'Jobscode', type: 'string' },
// { name: 'Xmldata', type: 'string' },
 { name: 'Cpyid', type: 'decimal' },
 { name: 'Cpyparrent', type: 'decimal' }
]
var Jobs_columns = [
 { text: '§Id_Jobs§', datafield: 'Id_Jobs' },
 { text: '§Id_Parent§', datafield: 'Id_Parent' },
 { text: '§Id_Ord§', datafield: 'Id_Ord' },
 { text: '§Name§', datafield: 'Name' },
 { text: '§Jobstype§', datafield: 'Jobstype' },
 { text: '§Jobscategory§', datafield: 'Jobscategory' },
 { text: '§Recordtype§', datafield: 'Recordtype' },
 { text: '§Jobscode§', datafield: 'Jobscode' },
 { text: '§Xmldata§', datafield: 'Xmldata' },
 { text: '§Cpyid§', datafield: 'Cpyid' },
 { text: '§Cpyparrent§', datafield: 'Cpyparrent' }
]

var Jobspropertyes_datafields = [
 { name: 'Id_Jobspropertyes', type: 'decimal' },
 { name: 'Id_Jobs', type: 'decimal' },
 { name: 'Id_Flows', type: 'Number' },
 { name: 'Cpyid', type: 'Number' },
 { name: 'Recordtype', type: 'string' },
 { name: 'Itemtype', type: 'string' },
 { name: 'Name', type: 'string' },
 { name: 'Description', type: 'string' },
{ name: 'Attributum', type: 'string' }
 //{ name: 'Xmldata', type: 'string' }
]
var Jobspropertyes_columns = [
 { text: '§Id_Jobspropertyes§', datafield: 'Id_Jobspropertyes', hidden: true },
 { text: '§Id_Jobs§', datafield: 'Id_Jobs' , hidden: true},
  { text: '§Id_Flows§', datafield: 'Id_Flows', hidden: true },
 //{ text: '§Cpyid§', datafield: 'Cpyid' },
 { text: '§Recordtype§', datafield: 'Recordtype', cellsrenderer: null },
 { text: '§Itemtype§', datafield: 'Itemtype', hidden: true },
 { text: '§Name§', datafield: 'Name' },
 { text: '§Description§', datafield: 'Description' },
 { text: '§Attributum§', datafield: 'Attributum', hidden: true}
  //{ text: '§Xmldata§', datafield: 'Xmldata' }
]

var Scopeofactivity_datafields = [
 { name: 'Id_Scopeofactivity', type: 'decimal' },
 { name: 'Id_Parent', type: 'decimal' },
 { name: 'Id_Ord', type: 'decimal' },
 { name: 'Id_Flows', type: 'decimal' },
 { name: 'Name', type: 'string' },
 { name: 'Type', type: 'string' },
 { name: 'Category', type: 'string' },
 { name: 'Recordtype', type: 'string' },
 { name: 'Code', type: 'string' },
 { name: 'Xmldata', type: 'string' },
 { name: 'Cpyid', type: 'decimal' },
 { name: 'Cpyparrent', type: 'decimal' }
]
var Scopeofactivity_columns = [
 { text: '§Id_Scopeofactivity§', datafield: 'Id_Scopeofactivity' },
 { text: '§Id_Parent§', datafield: 'Id_Parent' },
 { text: '§Id_Ord§', datafield: 'Id_Ord' },
 { text: '§Id_Flows§', datafield: 'Id_Flows' },
 { text: '§Name§', datafield: 'Name' },
 { text: '§Type§', datafield: 'Type' },
 { text: '§Category§', datafield: 'Category' },
 { text: '§Recordtype§', datafield: 'Recordtype' },
 { text: '§Code§', datafield: 'Code' },
 { text: '§Xmldata§', datafield: 'Xmldata' },
 { text: '§Cpyid§', datafield: 'Cpyid' },
 { text: '§Cpyparrent§', datafield: 'Cpyparrent' }
]

var Scopeofactivityproperties_datafields = [
 { name: 'Id_Scopeofactivityproperties', type: 'decimal' },
 { name: 'Id_Scopeofactivity', type: 'decimal' },
 { name: 'Id_Flows', type: 'decimal' },
 { name: 'Cpyid', type: 'decimal' },
 { name: 'Recordtype', type: 'string' },
 { name: 'Itemtype', type: 'string' },
 { name: 'Name', type: 'string' },
 { name: 'Description', type: 'string' },
 { name: 'Xmldata', type: 'string' }
]
var Scopeofactivityproperties_columns = [
 { text: '§Id_Scopeofactivityproperties§', datafield: 'Id_Scopeofactivityproperties' },
 { text: '§Id_Scopeofactivity§', datafield: 'Id_Scopeofactivity' },
 { text: '§Id_Flows§', datafield: 'Id_Flows' },
 { text: '§Cpyid§', datafield: 'Cpyid' },
 { text: '§Recordtype§', datafield: 'Recordtype' },
 { text: '§Itemtype§', datafield: 'Itemtype' },
 { text: '§Name§', datafield: 'Name' },
 { text: '§Description§', datafield: 'Description' },
 { text: '§Xmldata§', datafield: 'Xmldata' }
]

var Scopeofactivityrequirement_datafields = [
 { name: 'Id_Scopeofactivityrequirement', type: 'decimal' },
 { name: 'Id_Scopeofactivity', type: 'decimal' },
 { name: 'Id_Flows', type: 'decimal' },
 { name: 'Cpyid', type: 'decimal' },
 { name: 'Recordtype', type: 'string' },
 { name: 'Itemtype', type: 'string' },
 { name: 'Name', type: 'string' },
 { name: 'Description', type: 'string' },
 { name: 'Xmldata', type: 'string' }
 ]
var Scopeofactivityrequirement_columns = [
 { text: '§Id_Scopeofactivityrequirement§', datafield: 'Id_Scopeofactivityrequirement' },
 { text: '§Id_Scopeofactivity§', datafield: 'Id_Scopeofactivity' },
 { text: '§Id_Flows§', datafield: 'Id_Flows' },
 { text: '§Cpyid§', datafield: 'Cpyid' },
 { text: '§Recordtype§', datafield: 'Recordtype' },
 { text: '§Itemtype§', datafield: 'Itemtype' },
 { text: '§Name§', datafield: 'Name' },
 { text: '§Description§', datafield: 'Description' },
 { text: '§Xmldata§', datafield: 'Xmldata' }
]

var Workspacerequirement_datafields = [
 { name: 'Id_Workspacerequirement', type: 'decimal' },
 { name: 'Id_Organization', type: 'decimal' },
 { name: 'Id_Flows', type: 'decimal' },
 { name: 'Cpyid', type: 'decimal' },
 { name: 'Recordtype', type: 'string' },
 { name: 'Itemtype', type: 'string' },
 { name: 'Name', type: 'string' },
 { name: 'Description', type: 'string' },
 { name: 'Xmldata', type: 'string' }
]
var Workspacerequirement_columns = [
 { text: '§Id_Workspacerequirement§', datafield: 'Id_Workspacerequirement' },
 { text: '§Id_Organization§', datafield: 'Id_Organization' },
 { text: '§Id_Flows§', datafield: 'Id_Flows' },
 { text: '§Cpyid§', datafield: 'Cpyid' },
 { text: '§Recordtype§', datafield: 'Recordtype' },
 { text: '§Itemtype§', datafield: 'Itemtype' },
 { text: '§Name§', datafield: 'Name' },
 { text: '§Description§', datafield: 'Description' },
 { text: '§Xmldata§', datafield: 'Xmldata' }
]
