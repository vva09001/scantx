import axios from 'axios';
const { REACT_APP_BASE_URL } = process.env;

const request = axios.create({
  baseURL: REACT_APP_BASE_URL,
  timeout: 20000,
  headers: {
    'Content-Type': 'application/json',
    'Accept': 'application/json',
    'Access-Control-Allow-Origin': '*'
  }
});

// before send request
request.interceptors.request.use(
  config => {
    return config;
  },
  error => {
    return error;
  }
);

// after send request
request.interceptors.response.use(
  response => {
    return response;
  },
  error => {
    const response = JSON.parse(JSON.stringify(error));
    return response.response;
  }
);

export default request;
