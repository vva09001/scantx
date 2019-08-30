import request from 'helpers/request';

const getCompany = () => {
    return request({
        url: '/company/get-company',
        method: 'get'
    })
}

const editCompany = params => {
    return request({
        url: '/company/edit-company/',
        method: 'put',
        data: params
    })
}

const deleteCompany = id => {
    return request({
        url: '/company/delete-company/',
        method: 'post',
        params: {
            Cid: id
        }
    })
}

export { getCompany, editCompany, deleteCompany };