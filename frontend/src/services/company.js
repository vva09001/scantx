import request from 'helpers/request';

const get = () => {
    return request({
        url: '/company/get-company',
        method: 'get'
    })
}

const add = params => {
    return request({
        url: '/company/add-company/',
        method: 'post',
        data: params
    })
}

const edit = params => {
    return request({
        url: '/company/edit-company/',
        method: 'put',
        data: params
    })
}

const remove = id => {
    return request({
        url: '/company/delete-company/',
        method: 'post',
        params: {
            Cid: id
        }
    })
}

export { get, add, edit, remove };