import moment from 'moment';

const Date = time => {
    return moment(time).format('MMMM Do YYYY')
}

const Time = time => {
    return moment(time).format('h:mm:ss')
}

export {
    Date, Time
}