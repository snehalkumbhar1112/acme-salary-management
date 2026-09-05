import { useEffect, useState } from 'react'
import {
  getSalaryByCountry,
  getSalaryByDepartment,
  type SalaryByCountry,
  type SalaryByDepartment,
} from '../services/reportService'
function Reports() {
  const [report, setReport] = useState<SalaryByCountry[]>([])
  const [departmentReport, setDepartmentReport] =
  useState<SalaryByDepartment[]>([])
  const [loading, setLoading] = useState(true)
  const [error, setError] = useState('')

 const loadReport = async () => {
  try {
    setLoading(true)
    setError('')

    const [countryData, departmentData] =
      await Promise.all([
        getSalaryByCountry(),
        getSalaryByDepartment(),
      ])

    setReport(countryData)
    setDepartmentReport(departmentData)
  } catch (err) {
    console.error(err)
    setError('Unable to load salary reports.')
  } finally {
    setLoading(false)
  }
}
  useEffect(() => {
    loadReport()
  }, [])

  const formatAmount = (amount: number) => {
    return amount.toLocaleString(undefined, {
      minimumFractionDigits: 2,
      maximumFractionDigits: 2,
    })
  }

  return (
    <div>
      <div className="d-flex justify-content-between align-items-center mb-4">
        <div>
          <h2>Salary Reports</h2>

          <p className="text-muted mb-0">
            Analyze salary distribution across countries
          </p>
        </div>

        <button
          className="btn btn-outline-primary"
          onClick={loadReport}
          disabled={loading}
        >
          {loading ? 'Refreshing...' : 'Refresh'}
        </button>
      </div>

      {error && (
        <div className="alert alert-danger">
          {error}
        </div>
      )}

      <div className="card shadow-sm">
        <div className="card-body">
          <div className="d-flex justify-content-between align-items-center mb-4">
            <h5 className="mb-0">
              Salary by Country
            </h5>

            {!loading && (
              <span className="text-muted">
                {report.length} Countries
              </span>
            )}
          </div>

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
                Loading salary report...
              </p>
            </div>
          ) : (
            <div className="table-responsive">
              <table className="table table-hover align-middle">
                <thead className="table-light">
                  <tr>
                    <th>Country</th>
                    <th>Currency</th>
                    <th>Employees</th>
                    <th>Total Base Salary</th>
                    <th>Total Bonus</th>
                    <th>Total Compensation</th>
                  </tr>
                </thead>

                <tbody>
                  {report.length === 0 ? (
                    <tr>
                      <td
                        colSpan={6}
                        className="text-center py-4"
                      >
                        No salary data found.
                      </td>
                    </tr>
                  ) : (
                    report.map((item) => (
                      <tr key={item.country}>
                        <td>
                          <strong>
                            {item.country}
                          </strong>
                        </td>

                        <td>
                          <span className="badge bg-light text-dark border">
                            {item.currencyCode}
                          </span>
                        </td>

                        <td>
                          {item.employeeCount.toLocaleString()}
                        </td>

                        <td>
                          {formatAmount(
                            item.totalBaseSalary
                          )}{' '}
                          {item.currencyCode}
                        </td>

                        <td>
                          {formatAmount(
                            item.totalBonus
                          )}{' '}
                          {item.currencyCode}
                        </td>

                        <td>
                          <strong>
                            {formatAmount(
                              item.totalCompensation
                            )}{' '}
                            {item.currencyCode}
                          </strong>
                        </td>
                      </tr>
                    ))
                  )}
                </tbody>
              </table>
            </div>
          )}
        </div>
      </div>
      <div className="card shadow-sm mb-4">
  <div className="card-body">
    <div className="d-flex justify-content-between align-items-center mb-4">
      <h5 className="mb-0">
        Salary by Department
      </h5>

      {!loading && (
        <span className="text-muted">
          {departmentReport.length} Department/Currency Groups
        </span>
      )}
    </div>

    <div className="table-responsive">
      <table className="table table-hover align-middle">
        <thead className="table-light">
          <tr>
            <th>Department</th>
            <th>Currency</th>
            <th>Employees</th>
            <th>Total Base Salary</th>
            <th>Total Bonus</th>
            <th>Total Compensation</th>
          </tr>
        </thead>

        <tbody>
          {departmentReport.length === 0 ? (
            <tr>
              <td
                colSpan={6}
                className="text-center py-4"
              >
                No department salary data found.
              </td>
            </tr>
          ) : (
            departmentReport.map((item, index) => (
              <tr
                key={`${item.department}-${item.currencyCode}-${index}`}
              >
                <td>
                  <strong>
                    {item.department}
                  </strong>
                </td>

                <td>
                  <span className="badge bg-light text-dark border">
                    {item.currencyCode}
                  </span>
                </td>

                <td>
                  {item.employeeCount.toLocaleString()}
                </td>

                <td>
                  {formatAmount(
                    item.totalBaseSalary
                  )}{' '}
                  {item.currencyCode}
                </td>

                <td>
                  {formatAmount(
                    item.totalBonus
                  )}{' '}
                  {item.currencyCode}
                </td>

                <td>
                  <strong>
                    {formatAmount(
                      item.totalCompensation
                    )}{' '}
                    {item.currencyCode}
                  </strong>
                </td>
              </tr>
            ))
          )}
        </tbody>
      </table>
    </div>
  </div>
</div>
    </div>
  )
}

export default Reports