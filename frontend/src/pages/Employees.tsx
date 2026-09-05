import { useEffect, useState } from 'react'
interface EmployeesProps {
  onViewEmployee: (employeeId: number) => void
}
import {
  getEmployees,
  getCountries,
  getDepartments,
  type Employee,
  type EmployeeListResponse,
  type Country,
  type Department,
} from '../services/employeeService'

function Employees({ onViewEmployee }: EmployeesProps) {
  const [employees, setEmployees] = useState<Employee[]>([])
  const [loading, setLoading] = useState(false)
  const [error, setError] = useState('')

  // Search and filters
  const [search, setSearch] = useState('')
  const [countryId, setCountryId] = useState<number | undefined>()
  const [departmentId, setDepartmentId] = useState<number | undefined>()
  const [employmentStatus, setEmploymentStatus] = useState('')

  // Dropdown data
  const [countries, setCountries] = useState<Country[]>([])
  const [departments, setDepartments] = useState<Department[]>([])

  // Pagination
  const [page, setPage] = useState(1)
  const pageSize = 10

  const [result, setResult] =
    useState<EmployeeListResponse | null>(null)

  // Load countries and departments
  useEffect(() => {
    const loadFilterData = async () => {
      try {
        const [countryData, departmentData] = await Promise.all([
          getCountries(),
          getDepartments(),
        ])

        setCountries(countryData)
        setDepartments(departmentData)
      } catch (err) {
        console.error(err)
        setError('Unable to load filter data.')
      }
    }

    loadFilterData()
  }, [])

  // Load employees
  const loadEmployees = async () => {
    try {
      setLoading(true)
      setError('')

      const data = await getEmployees({
        page,
        pageSize,
        search: search.trim() || undefined,
        countryId,
        departmentId,
        employmentStatus:
          employmentStatus || undefined,
      })

      setEmployees(data.items)
      setResult(data)
    } catch (err) {
      console.error(err)
      setError('Unable to load employees.')
    } finally {
      setLoading(false)
    }
  }

  // Load employees when page changes
  useEffect(() => {
    loadEmployees()
  }, [page])

  // Apply filters
  const handleSearch = () => {
    setPage(1)

    // If already on page 1, reload manually
    if (page === 1) {
      loadEmployees()
    }
  }

  // Clear all filters
  const handleClear = () => {
    setSearch('')
    setCountryId(undefined)
    setDepartmentId(undefined)
    setEmploymentStatus('')
    setPage(1)

    if (page === 1) {
      setTimeout(() => {
        loadEmployees()
      }, 0)
    }
  }

  return (
    <div>

      {/* Page Header */}
      <div className="d-flex justify-content-between align-items-center mb-4">
        <div>
          <h2>Employees</h2>

          <p className="text-muted mb-0">
            Manage and view organization employees
          </p>
        </div>
      </div>

      {/* Filters */}
      <div className="card shadow-sm mb-4">

        <div className="card-body">

          <div className="row g-3">

            {/* Search */}
            <div className="col-md-4">

              <label className="form-label">
                Search
              </label>

              <input
                type="text"
                className="form-control"
                placeholder="Employee code, name or email"
                value={search}
                onChange={(e) => setSearch(e.target.value)}
                onKeyDown={(e) => {
                  if (e.key === 'Enter') {
                    handleSearch()
                  }
                }}
              />

            </div>

            {/* Country */}
            <div className="col-md-2">

              <label className="form-label">
                Country
              </label>

              <select
                className="form-select"
                value={countryId ?? ''}
                onChange={(e) =>
                  setCountryId(
                    e.target.value
                      ? Number(e.target.value)
                      : undefined
                  )
                }
              >
                <option value="">
                  All Countries
                </option>

                {countries.map((country) => (
                  <option
                    key={country.id}
                    value={country.id}
                  >
                    {country.name}
                  </option>
                ))}
              </select>

            </div>

            {/* Department */}
            <div className="col-md-2">

              <label className="form-label">
                Department
              </label>

              <select
                className="form-select"
                value={departmentId ?? ''}
                onChange={(e) =>
                  setDepartmentId(
                    e.target.value
                      ? Number(e.target.value)
                      : undefined
                  )
                }
              >
                <option value="">
                  All Departments
                </option>

                {departments.map((department) => (
                  <option
                    key={department.id}
                    value={department.id}
                  >
                    {department.name}
                  </option>
                ))}
              </select>

            </div>

            {/* Employment Status */}
            <div className="col-md-2">

              <label className="form-label">
                Status
              </label>

              <select
                className="form-select"
                value={employmentStatus}
                onChange={(e) =>
                  setEmploymentStatus(e.target.value)
                }
              >
                <option value="">
                  All Statuses
                </option>

                <option value="Active">
                  Active
                </option>

                <option value="On Leave">
                  On Leave
                </option>
              </select>

            </div>

            {/* Buttons */}
            <div className="col-md-2 d-flex align-items-end gap-2">

              <button
                className="btn btn-primary flex-grow-1"
                onClick={handleSearch}
                disabled={loading}
              >
                Search
              </button>

              <button
                className="btn btn-outline-secondary"
                onClick={handleClear}
                disabled={loading}
              >
                Clear
              </button>

            </div>

          </div>

        </div>
      </div>

      {/* Error */}
      {error && (
        <div className="alert alert-danger">
          {error}
        </div>
      )}

      {/* Employee Table */}
      <div className="card shadow-sm">

        <div className="card-body">

          {/* Table Header */}
          <div className="d-flex justify-content-between mb-3">

            <h5 className="mb-0">
              Employee List
            </h5>

            {result && (
              <span className="text-muted">
                Total: {result.totalCount.toLocaleString()}
              </span>
            )}

          </div>

          {/* Loading */}
          {loading ? (

            <div className="text-center py-5">

              <div
                className="spinner-border"
                role="status"
              >
                <span className="visually-hidden">
                  Loading...
                </span>
              </div>

              <p className="mt-2 text-muted">
                Loading employees...
              </p>

            </div>

          ) : (

            <div className="table-responsive">

              <table className="table table-hover align-middle">

                <thead className="table-light">

                  <tr>
                    <th>Employee Code</th>
                    <th>Name</th>
                    <th>Email</th>
                    <th>Country</th>
                    <th>Department</th>
                    <th>Job Title</th>
                  <th>Status</th>
<th>Action</th>
                  </tr>

                </thead>

                <tbody>

                  {employees.length === 0 ? (

                    <tr>
                      <td
                        colSpan={8}
                        className="text-center py-4"
                      >
                        No employees found.
                      </td>
                    </tr>

                  ) : (

                    employees.map((employee) => (

                      <tr key={employee.id}>

                        <td>
                          <strong>
                            {employee.employeeCode}
                          </strong>
                        </td>

                        <td>
                          {employee.fullName}
                        </td>

                        <td>
                          {employee.email}
                        </td>

                        <td>
                          {employee.country}

                          <br />

                          <small className="text-muted">
                            {employee.currencyCode}
                          </small>
                        </td>

                        <td>
                          {employee.department}
                        </td>

                        <td>
                          {employee.jobTitle}
                        </td>

                       <td>

  <span
    className={`badge ${
      employee.employmentStatus ===
      'Active'
        ? 'bg-success'
        : 'bg-secondary'
    }`}
  >
    {employee.employmentStatus}
  </span>

</td>

<td>
 <button
  className="btn btn-sm btn-outline-primary"
  onClick={() => onViewEmployee(employee.id)}
>
  View
</button>
</td>

                      </tr>

                    ))

                  )}

                </tbody>

              </table>

            </div>

          )}

          {/* Pagination */}
          {result && result.totalPages > 1 && (

            <div className="d-flex justify-content-between align-items-center mt-3">

              <button
                className="btn btn-outline-secondary"
                disabled={
                  page === 1 || loading
                }
                onClick={() =>
                  setPage(page - 1)
                }
              >
                Previous
              </button>

              <span>
                Page <strong>{result.page}</strong>{' '}
                of{' '}
                <strong>
                  {result.totalPages}
                </strong>
              </span>

              <button
                className="btn btn-outline-secondary"
                disabled={
                  page === result.totalPages ||
                  loading
                }
                onClick={() =>
                  setPage(page + 1)
                }
              >
                Next
              </button>

            </div>

          )}

        </div>
      </div>

    </div>
  )
}

export default Employees