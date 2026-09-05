import api from './api'

export interface SalaryByCountry {
  country: string
  currencyCode: string
  employeeCount: number
  totalBaseSalary: number
  totalBonus: number
  totalCompensation: number
}

export const getSalaryByCountry = async (): Promise<
  SalaryByCountry[]
> => {
  const response = await api.get<SalaryByCountry[]>(
    '/Dashboard/salary-by-country'
  )

  return response.data
}

export interface SalaryByDepartment {
  department: string
  currencyCode: string
  employeeCount: number
  totalBaseSalary: number
  totalBonus: number
  totalCompensation: number
}

export const getSalaryByDepartment = async (): Promise<
  SalaryByDepartment[]
> => {
  const response = await api.get<SalaryByDepartment[]>(
    '/Dashboard/salary-by-department'
  )

  return response.data
}