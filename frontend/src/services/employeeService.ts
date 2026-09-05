
import api from './api'

export interface Employee {
  id: number
  employeeCode: string
  fullName: string
  email: string
  country: string
  currencyCode: string
  department: string
  jobTitle: string
  employmentStatus: string
  hireDate: string
}

export interface EmployeeListResponse {
  items: Employee[]
  page: number
  pageSize: number
  totalCount: number
  totalPages: number
}

export interface EmployeeFilters {
  page?: number
  pageSize?: number
  search?: string
  countryId?: number
  departmentId?: number
  employmentStatus?: string
}

export interface Country {
  id: number
  name: string
  code: string
  currencyCode: string
}

export interface Department {
  id: number
  name: string
}

export const getEmployees = async (
  filters: EmployeeFilters = {}
): Promise<EmployeeListResponse> => {
  const response = await api.get<EmployeeListResponse>(
    '/Employees',
    {
      params: filters,
    }
  )

  return response.data
}

export const getCountries = async (): Promise<Country[]> => {
  const response = await api.get<Country[]>('/Countries')

  return response.data
}

export const getDepartments = async (): Promise<Department[]> => {
  const response = await api.get<Department[]>('/Departments')

  return response.data
}

export interface EmployeeDetail {
  id: number
  employeeCode: string
  firstName: string
  lastName: string
  email: string
  country: string
  countryCode: string
  currencyCode: string
  department: string
  jobTitle: string
  employmentStatus: string
  hireDate: string
  currentBaseSalary: number | null
  currentBonus: number | null
  salaryEffectiveFrom: string | null
  salaryChangeReason: string | null
}

export const getEmployeeById = async (
  id: number
): Promise<EmployeeDetail> => {
  const response = await api.get<EmployeeDetail>(
    `/Employees/${id}`
  )

  return response.data
}

export interface SalaryHistory {
  id: number
  baseSalary: number
  bonus: number
  currencyCode: string
  effectiveFrom: string
  effectiveTo: string | null
  changeReason: string
  createdAt: string
}

export const getSalaryHistory = async (
  employeeId: number
): Promise<SalaryHistory[]> => {
  const response = await api.get<SalaryHistory[]>(
    `/Employees/${employeeId}/salary-history`
  )

  return response.data
}

/* Update Salary */

export interface UpdateSalaryRequest {
  baseSalary: number
  bonus: number
  changeReason: string
  effectiveFrom: string
}

export interface SalaryResponse {
  id: number
  employeeId: number
  baseSalary: number
  bonus: number
  currencyCode: string
  effectiveFrom: string
  effectiveTo: string | null
  changeReason: string
  createdAt: string
}

export const updateEmployeeSalary = async (
  employeeId: number,
  salary: UpdateSalaryRequest
): Promise<SalaryResponse> => {
  const response = await api.post<SalaryResponse>(
    `/Employees/${employeeId}/salary`,
    salary
  )

  return response.data
}

export interface SalaryAudit {
  id: number
  salaryId: number
  employeeId: number
  previousSalary: number
  newSalary: number
  previousBonus: number
  newBonus: number
  changeReason: string
  changedAt: string
  changedBy: string
}

export const getSalaryAudit = async (
  employeeId: number
): Promise<SalaryAudit[]> => {
  const response = await api.get<SalaryAudit[]>(
    `/Employees/${employeeId}/salary-audit`
  )

  return response.data
}