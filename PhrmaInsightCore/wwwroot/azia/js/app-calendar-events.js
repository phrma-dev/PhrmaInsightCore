// sample calendar events data

'use strict';

var curYear = moment().format('YYYY');
var curMonth = moment().format('MM');
$.get({
    url: '../Kastle/GetVisitorGroups',
    success: function (data) {
        var events = [];
        $.each(data, function (k) {
            var obj = new Object();
            obj = {
                id:  (k + 1).toString(),
                start: data[k]["meetingDate"].toString().substring(0, 4) + '-' + data[k]["meetingDate"].toString().substring(4, 6) + '-' + data[k]["meetingDate"].toString().substring(6, 8) + 'T' + data[k]["meetingDate"].toString().substring(9, 11) + ':' +  data[k]["meetingDate"].toString().substring(11, 13) + ':00',
                end: data[k]["meetingDate"].toString().substring(0, 4) + '-' + data[k]["meetingDate"].toString().substring(4, 6) + '-' + data[k]["meetingDate"].toString().substring(6, 8) + 'T11:59:59',
                title: data[k]["subject"].toString(),
                backgroundColor: '#bff2f2',
                borderColor: '#00cccc',
                description: ''
            };
            events.push(obj);
            go(events);
        });
        // END EACH
        go(events);
    }
        // END SUCCESS
});

var azCalendarEvents = new Object();
var azBirthdayEvents = new Object();
var azHolidayEvents = new Object();
var azOtherEvents = new Object();
// END GET VISITORS
function go(eventList) {
    console.log(eventList);
    azCalendarEvents = {
        id: 1,
        backgroundColor: '#cbfbb0',
        borderColor: '#3bb001',
        events: eventList
        //events: [
        //  {
        //    id: '1',
        //    start: curYear+'-'+curMonth+'-08T08:30:00',
        //    end: curYear+'-'+curMonth+'-08T13:00:00',
        //    title: 'BootstrapDash Meetup',
        //    backgroundColor: '#bff2f2',
        //    borderColor: '#00cccc',
        //    description: 'In enim justo, rhoncus ut, imperdiet a, venenatis vitae, justo. Nullam dictum felis az pede mollis...'
        //  },{
        //    id: '2',
        //    start: curYear+'-'+curMonth+'-10T09:00:00',
        //    end: curYear+'-'+curMonth+'-10T17:00:00',
        //    title: 'Design Review',
        //    backgroundColor: '#e0e4f4',
        //    borderColor: '#0a2ba5',
        //    description: 'In enim justo, rhoncus ut, imperdiet a, venenatis vitae, justo. Nullam dictum felis az pede mollis...'
        //  },{
        //    id: '3',
        //    start: curYear+'-'+curMonth+'-13T12:00:00',
        //    end: curYear+'-'+curMonth+'-13T18:00:00',
        //    title: 'Lifestyle Conference',
        //    backgroundColor: '#ffd5cc',
        //    borderColor: '#ff5733',
        //    description: 'Aenean imperdiet. Etiam ultricies nisi vel augue. Curabitur ullamcorper ultricies nisi...'
        //  },{
        //    id: '4',
        //    start: curYear+'-'+curMonth+'-15T07:30:00',
        //    end: curYear+'-'+curMonth+'-15T15:30:00',
        //    title: 'Team Weekly Brownbag',
        //    backgroundColor: '#d2e0ff',
        //    borderColor: '#0373f3',
        //    description: 'In enim justo, rhoncus ut, imperdiet a, venenatis vitae, justo. Nullam dictum felis az pede mollis...'
        //  },{
        //    id: '5',
        //    start: curYear+'-'+curMonth+'-17T10:00:00',
        //    end: curYear+'-'+curMonth+'-19T15:00:00',
        //    title: 'Music Festival',
        //    backgroundColor: '#bfdeff',
        //    borderColor: '#007bff',
        //    description: 'In enim justo, rhoncus ut, imperdiet a, venenatis vitae, justo. Nullam dictum felis az pede mollis...'
        //  },{
        //    id: '6',
        //    start: curYear+'-'+curMonth+'-08T13:00:00',
        //    end: curYear+'-'+curMonth+'-08T18:30:00',
        //    title: 'Attend Lea\'s Wedding',
        //    backgroundColor: '#d5c2f3',
        //    borderColor: '#560bd0',
        //    description: 'In enim justo, rhoncus ut, imperdiet a, venenatis vitae, justo. Nullam dictum felis az pede mollis...'
        //  }
        //]
    };

    // Birthday Events Source
    azBirthdayEvents = {
        id: 2,
        backgroundColor: '#cbfbb0',
        borderColor: '#3bb001',
        events: [
            {
                id: '7',
                start: curYear + '-' + curMonth + '-01T18:00:00',
                end: curYear + '-' + curMonth + '-01T23:30:00',
                title: 'Socrates Birthday',
                backgroundColor: '#d8fed1',
                borderColor: '#23bf08',
                description: 'In enim justo, rhoncus ut, imperdiet a, venenatis vitae, justo. Nullam dictum felis az pede mollis...'
            },
            {
                id: '8',
                start: curYear + '-' + curMonth + '-21T15:00:00',
                end: curYear + '-' + curMonth + '-21T21:00:00',
                title: 'Reynante\'s Birthday',
                description: 'In enim justo, rhoncus ut, imperdiet a, venenatis vitae, justo. Nullam dictum felis az pede mollis...'
            },
            {
                id: '9',
                start: curYear + '-' + curMonth + '-23T15:00:00',
                end: curYear + '-' + curMonth + '-23T21:00:00',
                title: 'Pauline\'s Birthday',
                description: 'In enim justo, rhoncus ut, imperdiet a, venenatis vitae, justo. Nullam dictum felis az pede mollis...'
            }
        ]
    };


    azHolidayEvents = {
        id: 3,
        backgroundColor: '#fbbfdc',
        borderColor: '#f10075',
        events: [
            {
                id: '10',
                start: curYear + '-' + curMonth + '-04',
                end: curYear + '-' + curMonth + '-06',
                title: 'Feast Day'
            },
            {
                id: '11',
                start: curYear + '-' + curMonth + '-26',
                end: curYear + '-' + curMonth + '-27',
                title: 'Memorial Day'
            },
            {
                id: '12',
                start: curYear + '-' + curMonth + '-28',
                end: curYear + '-' + curMonth + '-29',
                title: 'Veteran\'s Day'
            }
        ]
    };


    azOtherEvents = {
        id: 4,
        backgroundColor: '#ffecca',
        borderColor: '#ffb52b',
        events: [
            {
                id: '13',
                start: curYear + '-' + curMonth + '-06',
                end: curYear + '-' + curMonth + '-08',
                title: 'My Rest Day'
            },
            {
                id: '13',
                start: curYear + '-' + curMonth + '-29',
                end: curYear + '-' + curMonth + '-31',
                title: 'My Rest Day'
            }
        ]
    }; console.log(azCalendarEvents);
    return azCalendarEvents;
}