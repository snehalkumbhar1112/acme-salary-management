import axios from 'axios'

const api = axios.create({
  baseURL: 'https://acme-salary-management-hfgm.onrender.com/api',
  headers: {
    'Content-Type': 'application/json',
  },
})

export default api