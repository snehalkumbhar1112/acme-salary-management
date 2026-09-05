import { useEffect, useState } from 'react'
import Employees from './pages/Employees'
import EmployeeDetails from './pages/EmployeeDetails'
import Reports from './pages/Reports'
import api from './services/api'


interface DashboardSummary {
  totalEmployees: number
  activeEmployees: number
  countries: number
  departments: number
}

function App() {
  const [summary, setSummary] =
    useState<DashboardSummary | null>(null)

  const [loading, setLoading] =
    useState(true)

  const [error, setError] =
    useState('')

  const [currentPage, setCurrentPage] =
    useState('dashboard')

  const [selectedEmployeeId, setSelectedEmployeeId] =
    useState<number | null>(null)

  useEffect(() => {
    const loadDashboard = async () => {
      try {
        const response =
          await api.get<DashboardSummary>(
            '/Dashboard/summary'
          )

        setSummary(response.data)
      } catch (err) {
        console.error(err)
        setError(
          'Unable to load dashboard data.'
        )
      } finally {
        setLoading(false)
      }
    }

    loadDashboard()
  }, [])

  return (
    <div className="container-fluid">
      <div className="row">

        {/* ============================= */}
        {/* Sidebar */}
        {/* ============================= */}

        <div className="col-md-2 bg-dark text-white min-vh-100 p-3">

          <h4 className="mb-4">
            Salary Manager
          </h4>

          <div className="d-grid gap-2">

            {/* Dashboard */}
            <button
              className={`btn text-start ${
                currentPage === 'dashboard'
                  ? 'btn-primary'
                  : 'btn-dark'
              }`}
              onClick={() =>
                setCurrentPage('dashboard')
              }
            >
              Dashboard
            </button>

            {/* Employees */}
            <button
              className={`btn text-start ${
                currentPage === 'employees' ||
                currentPage === 'employee-details'
                  ? 'btn-primary'
                  : 'btn-dark'
              }`}
              onClick={() =>
                setCurrentPage('employees')
              }
            >
              Employees
            </button>

            {/* Future */}
            <button className="btn btn-dark text-start">
              Salary Management
            </button>

            {/* Future */}
         <button
  className={`btn text-start ${
    currentPage === 'reports'
      ? 'btn-primary'
      : 'btn-dark'
  }`}
  onClick={() => setCurrentPage('reports')}
>
  Reports
</button>

          </div>
        </div>

        {/* ============================= */}
        {/* Main Content */}
        {/* ============================= */}

        <div className="col-md-10 p-4">

          {/* ============================= */}
          {/* EMPLOYEES PAGE */}
          {/* ============================= */}

          {currentPage === 'employees' && (
            <Employees
              onViewEmployee={(employeeId) => {
                setSelectedEmployeeId(employeeId)
                setCurrentPage('employee-details')
              }}
            />
          )}

          {/* ============================= */}
          {/* EMPLOYEE DETAILS PAGE */}
          {/* ============================= */}

          {currentPage === 'employee-details' &&
            selectedEmployeeId !== null && (
              <EmployeeDetails
                employeeId={selectedEmployeeId}
                onBack={() => {
                  setCurrentPage('employees')
                }}
              />
            )}
{currentPage === 'reports' && (
  <Reports />
)}
          {/* ============================= */}
          {/* DASHBOARD PAGE */}
          {/* ============================= */}

          {currentPage === 'dashboard' && (
            <>
              <h2 className="mb-4">
                Dashboard
              </h2>

              {/* Error */}
              {error && (
                <div className="alert alert-danger">
                  {error}
                </div>
              )}

              {/* Loading */}
              {loading && (
                <div className="text-center py-5">

                  <div
                    className="spinner-border"
                    role="status"
                  >
                    <span className="visually-hidden">
                      Loading...
                    </span>
                  </div>

                  <p className="mt-2">
                    Loading dashboard...
                  </p>

                </div>
              )}

              {/* Dashboard Cards */}
              {!loading && summary && (
                <div className="row g-4">

                  {/* Total Employees */}
                  <div className="col-md-3">
                    <div className="card shadow-sm h-100">
                      <div className="card-body">

                        <h6 className="text-muted">
                          Total Employees
                        </h6>

                        <h2>
                          {summary.totalEmployees.toLocaleString()}
                        </h2>

                      </div>
                    </div>
                  </div>

                  {/* Active Employees */}
                  <div className="col-md-3">
                    <div className="card shadow-sm h-100">
                      <div className="card-body">

                        <h6 className="text-muted">
                          Active Employees
                        </h6>

                        <h2>
                          {summary.activeEmployees.toLocaleString()}
                        </h2>

                      </div>
                    </div>
                  </div>

                  {/* Countries */}
                  <div className="col-md-3">
                    <div className="card shadow-sm h-100">
                      <div className="card-body">

                        <h6 className="text-muted">
                          Countries
                        </h6>

                        <h2>
                          {summary.countries}
                        </h2>

                      </div>
                    </div>
                  </div>

                  {/* Departments */}
                  <div className="col-md-3">
                    <div className="card shadow-sm h-100">
                      <div className="card-body">

                        <h6 className="text-muted">
                          Departments
                        </h6>

                        <h2>
                          {summary.departments}
                        </h2>

                      </div>
                    </div>
                  </div>

                </div>
              )}

              {/* Welcome */}
              <div className="card shadow-sm mt-4">
                <div className="card-body">

                  <h5>
                    Welcome to Salary Management
                  </h5>

                  <p className="text-muted mb-0">
                    Manage employee information,
                    salaries, salary history, and
                    salary changes across the
                    organization.
                  </p>

                </div>
              </div>

            </>
          )}

        </div>
      </div>
    </div>
  )
}

export default App