
function httpGet(theUrl) {
    var xmlHttp = new XMLHttpRequest();
    xmlHttp.open("GET", theUrl, false); // false for synchronous request
    xmlHttp.send(null);
    return xmlHttp.responseText;
}


var value = window.location.href.split("=")[1].toString();

    

var query = "?department=" + value.replace("&", "amp");

var departmentHeadEmail;

var dept_head_url = '../HR/FindDepartmentHead' + query;
if (value === "All") {
    departmentHeadEmail = "";
} else {
    departmentHeadEmail = httpGet(dept_head_url);
}


var url = '../HR/Get_Employees' + query;

var data = JSON.parse(httpGet(url));



var dataArray = [];
for (var k = 0; k < data.length; k++) {
    var obj = new Object();
    var user_photo = data[k]["photo"] === "" ? '../../img/nophoto.png' : data[k]["photo"];
    if (data[k]["title"] === "President & CEO"  && value === "All") {
        obj =
            new Object({
                id: data[k]["email"],
                name: data[k]["name"],
                post: data[k]["title"].replace('&', 'and'),
                phone: data[k]["mobilePhone"],
                mail: data[k]["email"],
                photo: user_photo,
                department: data[k]["department"].replace('&', 'and')
            });
        obj.id = "main";
        dataArray.push(obj);

    } else if (data[k]["managerEmail"] === "subl@phrma.org" && value === "All") {
        obj =
            new Object({
                id: data[k]["email"],
                name: data[k]["name"],
                post: data[k]["title"].replace('&', 'and'),
                phone: data[k]["mobilePhone"],
                mail: data[k]["email"],
                photo: user_photo,
                parent: "main",
                dir: "vertical",
                open: false,
                department: data[k]["department"].replace('&', 'and')
            });
        dataArray.push(obj);

    } else if (data[k]["managerEmail"] !== "" && value === "All") {
        obj =
            new Object({
                id: data[k]["email"],
                name: data[k]["name"],
                post: data[k]["title"].replace('&', 'and'),
                phone: data[k]["mobilePhone"],
                mail: data[k]["email"],
                photo: user_photo,
                parent: data[k]["managerEmail"],
                dir: "vertical",
                open: false,
                department: data[k]["department"].replace('&', 'and')

            });
        dataArray.push(obj);

    } else if (data[k]["email"] === departmentHeadEmail && value !== "All") {
        obj =
            new Object({
                id: "main",
                name: data[k]["name"],
                post: data[k]["title"].replace('&', 'and'),
                phone: data[k]["mobilePhone"],
                mail: data[k]["email"],
                photo: user_photo,
                open: false,
                department: data[k]["department"].replace('&', 'and')
            });
        dataArray.push(obj);
    } else if (data[k]["email"] !== departmentHeadEmail && data[k]["managerEmail"] === departmentHeadEmail && value !== "All") {
        obj =
            new Object({
                id: data[k]["email"],
                name: data[k]["name"],
                post: data[k]["title"].replace('&', 'and'),
                phone: data[k]["mobilePhone"],
                mail: data[k]["email"],
                photo: user_photo,
                parent: "main",
                open: false,
                department: data[k]["department"].replace('&', 'and')
            });
        dataArray.push(obj);
    } else if (data[k]["email"] !== departmentHeadEmail && data[k]["managerEmail"] !== departmentHeadEmail && value !== "All") {
        obj =
            new Object({
                id: data[k]["email"],
                name: data[k]["name"],
                post: data[k]["title"].replace('&', 'and'),
                phone: data[k]["mobilePhone"],
                mail: data[k]["email"],
                photo: user_photo,
                parent: data[k]["managerEmail"],
                open: false,
                department: data[k]["department"].replace('&', 'and')
            });
        dataArray.push(obj);

    }
};
console.log(dataArray);
var medCardShape = dataArray;


    //var medCardShape = [
    //	{
    //		id: "main",
    //		name: "Kristin Mccoy",
    //		post: "Medical director",
    //		phone: "(405) 555-0128",
    //		mail: "kmccoy@gmail.com",
    //		photo: "../common/big_img/big-avatar-1.jpg",
    //		birthday: "10.01.1956",
    //		start: "15.09.1990"
    //	},
    //	{
    //		id: "jbean",
    //		name: "Theo Fisher",
    //		post: "Head of department",
    //		phone: "(405) 632-1372",
    //		mail: "tfisher@gmail.com",
    //		photo: "../common/big_img/big-avatar-2.jpg",
    //		parent: "main",
    //		birthday: "09.12.1987",
    //		start: "04.02.2018"
    //	}];
    //	{
    //		id: "1.1",
    //		name: "Francesca Saunders",
    //		post: "Attending physician",
    //		phone: "(402) 371-6736",
    //		mail: "fsaunders@gmail.com",
    //		photo: "../common/big_img/big-avatar-3.jpg",
    //		parent: "1",
    //		birthday: "25.05.1997",
    //		start: "12.09.2019"
    //	},
    //	{
    //		id: "1.1.1",
    //		name: "Jenson Brown",
    //		post: "Fellow",
    //		phone: "(346) 622-8633",
    //		mail: "jbrown@gmail.com",
    //		photo: "../common/big_img/big-avatar-14.jpg",
    //		parent: "1.1",
    //		dir: "vertical",
    //		birthday: "03.10.1970",
    //		start: "06.02.1998"
    //	},
    //	{
    //		id: "1.1.1.1",
    //		name: "Raya Marshall",
    //		post: "Resident",
    //		phone: "(846) 962-1723",
    //		mail: "rmarshall@gmail.com",
    //		photo: "../common/big_img/big-avatar-16.jpg",
    //		parent: "1.1.1",
    //		birthday: "02.11.1984",
    //		start: "12.06.2015"
    //	},
    //	{
    //		id: "1.1.1.2",
    //		name: "Tom Walsh",
    //		post: "Resident",
    //		phone: "(763) 213-8373",
    //		mail: "twalsh@gmail.com",
    //		photo: "../common/big_img/big-avatar-17.jpg",
    //		parent: "1.1.1",
    //		birthday: "15.03.1978",
    //		start: "15.08.1999"
    //	},
    //	{
    //		id: "1.1.1.3",
    //		name: "Harvey Pearce",
    //		post: "Resident",
    //		phone: "(364) 234-7523",
    //		mail: "hpearce@gmail.com",
    //		photo: "../common/big_img/big-avatar-18.jpg",
    //		parent: "1.1.1",
    //		birthday: "12.12.1990",
    //		start: "05.03.2017"
    //	},
    //	{
    //		id: "1.1.2",
    //		name: "Archie Barnes",
    //		post: "Fellow",
    //		phone: "(578) 342-1237",
    //		mail: "abarnes@gmail.com",
    //		photo: "../common/big_img/big-avatar-19.jpg",
    //		parent: "1.1",
    //		dir: "vertical",
    //		birthday: "15.03.1954",
    //		start: "09.12.1986"
    //	},
    //	{
    //		id: "1.1.2.1",
    //		name: "Emelia Green",
    //		post: "Resident",
    //		phone: "(832) 426-2223",
    //		mail: "egreen@gmail.com",
    //		photo: "../common/big_img/big-avatar-20.jpg",
    //		parent: "1.1.2",
    //		birthday: "01.06.1957",
    //		start: "12.02.1976"
    //	},
    //	{
    //		id: "1.1.2.2",
    //		name: "Dylan Barrett",
    //		post: "Resident",
    //		phone: "(523) 125-2523",
    //		mail: "dbarrett@gmail.com",
    //		photo: "../common/big_img/big-avatar-21.jpg",
    //		parent: "1.1.2",
    //		birthday: "09.09.1948",
    //		start: "04.03.1968"
    //	},
    //	{
    //		id: "1.1.3",
    //		name: "Abraham Johnston",
    //		post: "Fellow",
    //		phone: "(251) 315-4731",
    //		mail: "ajohnston@gmail.com",
    //		photo: "../common/big_img/big-avatar-22.jpg",
    //		parent: "1.1",
    //		dir: "vertical",
    //		birthday: "01.07.1956",
    //		start: "09.01.2020"
    //	},
    //	{
    //		id: "1.1.3.1",
    //		name: "Philippa Holmes",
    //		post: "Resident",
    //		phone: "(151) 231-1256",
    //		mail: "pholmes@gmail.com",
    //		photo: "../common/big_img/big-avatar-23.jpg",
    //		parent: "1.1.3",
    //		birthday: "07.01.2000",
    //		start: "25.12.2019"
    //	},
    //	{
    //		id: "1.1.3.2",
    //		name: "Claudia Fraser",
    //		post: "Resident",
    //		phone: "(125) 215-2636",
    //		mail: "cfraser@gmail.com",
    //		photo: "../common/big_img/big-avatar-24.jpg",
    //		parent: "1.1.3",
    //		birthday: "05.08.1988",
    //		start: "07.06.2010"
    //	},
    //	{
    //		id: "2",
    //		name: "Alisha Hall",
    //		post: "Head of department",
    //		phone: "(405) 372-9756",
    //		mail: "ahall@gmail.com",
    //		photo: "../common/big_img/big-avatar-4.jpg",
    //		parent: "main",
    //		birthday: "04.04.1983",
    //		start: "05.03.2015"
    //	},
    //	{
    //		id: "2.1",
    //		name: "Milena Hunter",
    //		post: "Attending physician",
    //		phone: "(124) 622-1256",
    //		mail: "mhunter@gmail.com",
    //		photo: "../common/big_img/big-avatar-25.jpg",
    //		parent: "2",
    //		dir: "vertical",
    //		birthday: "04.03.1989",
    //		start: "09.01.2020"
    //	},
    //	{
    //		id: "2.1.1",
    //		name: "Bradley Sutton",
    //		post: "Fellow",
    //		phone: "(325) 154-6232",
    //		mail: "bsutton@gmail.com",
    //		photo: "../common/big_img/big-avatar-26.jpg",
    //		parent: "2.1",
    //		birthday: "01.12.1993",
    //		start: "11.05.2019"
    //	},
    //	{
    //		id: "2.1.2",
    //		name: "Joel Stevens",
    //		post: "Fellow",
    //		phone: "(165) 463-1232",
    //		mail: "jstevens@gmail.com",
    //		photo: "../common/big_img/big-avatar-27.jpg",
    //		parent: "2.1",
    //		birthday: "25.11.1986",
    //		start: "07.03.2005"
    //	},
    //	{
    //		id: "2.1.3",
    //		name: "Axel Khan",
    //		post: "Fellow",
    //		phone: "(578) 734-3633",
    //		mail: "akhan@gmail.com",
    //		photo: "../common/big_img/big-avatar-28.jpg",
    //		parent: "2.1",
    //		birthday: "03.03.1987",
    //		start: "09.05.2014"
    //	},
    //	{
    //		id: "2.2",
    //		name: "Maximus Dixon",
    //		post: "Medical director",
    //		phone: "(264) 684-4373",
    //		mail: "mdixon@gmail.com",
    //		photo: "../common/big_img/big-avatar-29.jpg",
    //		parent: "2",
    //		dir: "vertical",
    //		birthday: "09.11.1986",
    //		start: "05.05.2014"
    //	},
    //	{
    //		id: "2.2.1",
    //		name: "Sami Morris",
    //		post: "Fellow",
    //		phone: "(437) 347-3473",
    //		mail: "smorris@gmail.com",
    //		photo: "../common/big_img/big-avatar-30.jpg",
    //		parent: "2.2",
    //		birthday: "25.11.1986",
    //		start: "07.03.2005"
    //	},
    //	{
    //		id: "2.2.2",
    //		name: "Jessica Murray",
    //		post: "Fellow",
    //		phone: "(436) 348-8692",
    //		mail: "jmurray@gmail.com",
    //		photo: "../common/big_img/big-avatar-31.jpg",
    //		parent: "2.2",
    //		birthday: "03.02.1979",
    //		start: "12.10.2015"
    //	},
    //	{
    //		id: "2.2.3",
    //		name: "Maryam Barker",
    //		post: "Fellow",
    //		phone: "(632) 324-3262",
    //		mail: "mbarker@gmail.com",
    //		photo: "../common/big_img/big-avatar-32.jpg",
    //		parent: "2.2",
    //		birthday: "02.12.1954",
    //		start: "01.03.2005"
    //	},
    //	{
    //		id: "3",
    //		name: "Edward Sharp",
    //		post: "Head of department",
    //		phone: "(451) 251-2578",
    //		mail: "esharp@gmail.com",
    //		photo: "../common/big_img/big-avatar-6.jpg",
    //		parent: "main",
    //		dir: "vertical",
    //		birthday: "02.02.1960",
    //		start: "09.01.2000"
    //	},
    //	{
    //		id: "3.1",
    //		name: "Cruz Burke",
    //		post: "Attending physician",
    //		phone: "(587) 234-8975",
    //		mail: "cburke@gmail.com",
    //		photo: "../common/big_img/big-avatar-7.jpg",
    //		parent: "3",
    //		birthday: "01.04.1997",
    //		start: "01.02.2019"
    //	},
    //	{
    //		id: "3.2",
    //		name: "Eloise Saunders",
    //		post: "Attending physician",
    //		phone: "(875) 231-5332",
    //		mail: "esaunders@gmail.com",
    //		photo: "../common/big_img/big-avatar-8.jpg",
    //		parent: "3",
    //		birthday: "25.04.1987",
    //		start: "09.09.2018"
    //	},
    //	{
    //		id: "3.3",
    //		name: "Sophia Matthews",
    //		post: "Attending physician",
    //		phone: "(361) 423-7234",
    //		mail: "smatthews@gmail.com",
    //		photo: "../common/big_img/big-avatar-9.jpg",
    //		parent: "3",
    //		birthday: "01.03.1986",
    //		start: "01.01.2015"
    //	},
    //	{
    //		id: "3.4",
    //		name: "Kara Foster",
    //		post: "Attending physician",
    //		phone: "(565) 525-0672",
    //		mail: "kfoster@gmail.com",
    //		photo: "../common/big_img/big-avatar-10.jpg",
    //		parent: "3",
    //		birthday: "05.12.1980",
    //		start: "06.01.2000"
    //	},
    //	{
    //		id: "4",
    //		name: "Peter Cox",
    //		post: "Head of department",
    //		phone: "(732) 321-2312",
    //		mail: "pcox@gmail.com",
    //		photo: "../common/big_img/big-avatar-11.jpg",
    //		parent: "main",
    //		dir: "vertical",
    //		birthday: "09.03.1964",
    //		start: "03.01.2002"
    //	},
    //	{
    //		id: "4.1",
    //		name: "Nancy Collins",
    //		post: "Attending physician",
    //		phone: "(743) 235-1263",
    //		mail: "ncollins@gmail.com",
    //		photo: "../common/big_img/big-avatar-12.jpg",
    //		parent: "4",
    //		birthday: "04.06.1988",
    //		start: "09.03.2014"
    //	},
    //	{
    //		id: "4.1.1",
    //		name: "Alyssa Day",
    //		post: "Fellow",
    //		phone: "(623) 265-2362",
    //		mail: "aday@gmail.com",
    //		photo: "../common/big_img/big-avatar-33.jpg",
    //		parent: "4.1",
    //		birthday: "03.03.1989",
    //		start: "03.02.2019"
    //	},
    //	{
    //		id: "4.1.2",
    //		name: "Nancy Newman",
    //		post: "Fellow",
    //		phone: "(347) 236-2373",
    //		mail: "nnewman@gmail.com",
    //		photo: "../common/big_img/big-avatar-34.jpg",
    //		parent: "4.1",
    //		birthday: "09.10.1980",
    //		start: "09.09.2000"
    //	},
    //	{
    //		id: "4.2",
    //		name: "Honey Black",
    //		post: "Attending physician",
    //		phone: "(263) 234-8756",
    //		mail: "hblack@gmail.com",
    //		photo: "../common/big_img/big-avatar-13.jpg",
    //		parent: "4",
    //		birthday: "12.12.1989",
    //		start: "04.07.2016"
    //	},
    //	{
    //		id: "4.3",
    //		name: "Archie Moore",
    //		post: "Attending physician",
    //		phone: "(705) 236-5742",
    //		mail: "amoore@gmail.com",
    //		photo: "../common/big_img/big-avatar-5.jpg",
    //		parent: "4",
    //		birthday: "02.12.1990",
    //		start: "09.03.2015"
    //	}
    //];

