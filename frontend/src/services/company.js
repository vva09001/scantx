import request from 'helpers/request';

const get = (token) => {
    return request({
        url: '/company/get-company',
        method: 'get',
        headers: {
            Authorization: `Bearer ${token}`
        },      
    })
}

const add = (params, token) => {
    return request({
        url: '/company/add-company/',
        method: 'post',
        data: params,
        headers: {
            Authorization: `Bearer ${token}`
        }, 
    })
}

const edit = (params, token) => {
    return request({
        url: '/company/edit-company/',
        method: 'put',
        data: params,
        headers: {
            Authorization: `Bearer ${token}`
        }, 
    })
}

const remove = (data, token) => {
    return request({
        url: '/company/delete-arr-company/',
        method: 'post',
        data: data,
        headers: {
            Authorization: `Bearer ${token}`
        }, 
    })
}

const assign = (id, data, token) => {
    return request({
        url: '/company/assign/',
        method: 'post',
        params: {
            cid: id
        },
        data: data,
        headers: {
            Authorization: `Bearer ${token}`
        }, 
    })
}

export { get, add, edit, remove, assign };