
import { useEffect, useState } from 'react'
import {
  getEmployeeById,
  getSalaryHistory,
  getSalaryAudit,
  updateEmployeeSalary,
  type EmployeeDetail,
  type SalaryHistory,
  type SalaryAudit,
} from '../services/employeeService'

interface EmployeeDetailsProps {
  employeeId: number
  onBack: () => void
}

function EmployeeDetails({
  employeeId,
  onBack,
}: EmployeeDetailsProps) {
  const [employee, setEmployee] =
    useState<EmployeeDetail | null>(null)

  const [salaryHistory, setSalaryHistory] =
    useState<SalaryHistory[]>([])

  const [salaryAudit, setSalaryAudit] =
    useState<SalaryAudit[]>([])

  const [loading, setLoading] = useState(true)
  const [error, setError] = useState('')

  // Salary form
  const [baseSalary, setBaseSalary] = useState('')
  const [bonus, setBonus] = useState('')
  const [effectiveFrom, setEffectiveFrom] =
    useState('')
  const [changeReason, setChangeReason] =
    useState('')

  const [savingSalary, setSavingSalary] =
    useState(false)

  const [salarySuccess, setSalarySuccess] =
    useState('')

  const [salaryError, setSalaryError] =
    useState('')

  const loadEmployee = async () => {
    try {
      setLoading(true)
      setError('')

      const [employeeData, historyData, auditData] =
        await Promise.all([
          getEmployeeById(employeeId),
          getSalaryHistory(employeeId),
          getSalaryAudit(employeeId),
        ])

      setEmployee(employeeData)
      setSalaryHistory(historyData)
      setSalaryAudit(auditData)

      // Set form defaults from current salary
      setBaseSalary(
        employeeData.currentBaseSalary !== null
          ? employeeData.currentBaseSalary.toString()
          : ''
      )

      setBonus(
        employeeData.currentBonus !== null
          ? employeeData.currentBonus.toString()
          : ''
      )

      setEffectiveFrom(
        new Date().toISOString().substring(0, 10)
      )

      setChangeReason('')
    } catch (err) {
      console.error(err)

      setError(
        'Unable to load employee details.'
      )
    } finally {
      setLoading(false)
    }
  }

  useEffect(() => {
    loadEmployee()
  }, [employeeId])

  const handleSalaryUpdate = async () => {
    setSalarySuccess('')
    setSalaryError('')

    if (!baseSalary.trim()) {
      setSalaryError(
        'Base salary is required.'
      )
      return
    }

    if (!bonus.trim()) {
      setSalaryError(
        'Bonus is required.'
      )
      return
    }

    if (!effectiveFrom) {
      setSalaryError(
        'Effective date is required.'
      )
      return
    }

    if (!changeReason.trim()) {
      setSalaryError(
        'Change reason is required.'
      )
      return
    }

    const baseSalaryValue =
      Number(baseSalary)

    const bonusValue =
      Number(bonus)

    if (
      Number.isNaN(baseSalaryValue) ||
      baseSalaryValue < 0
    ) {
      setSalaryError(
        'Base salary must be a valid non-negative number.'
      )
      return
    }

    if (
      Number.isNaN(bonusValue) ||
      bonusValue < 0
    ) {
      setSalaryError(
        'Bonus must be a valid non-negative number.'
      )
      return
    }

    try {
      setSavingSalary(true)

      await updateEmployeeSalary(
        employeeId,
        {
          baseSalary: baseSalaryValue,
          bonus: bonusValue,
          changeReason: changeReason.trim(),
          effectiveFrom: `${effectiveFrom}T00:00:00`,
        }
      )

      setSalarySuccess(
        'Salary updated successfully.'
      )

      await loadEmployee()
    } catch (err) {
      console.error(err)

      setSalaryError(
        'Unable to update salary. Please try again.'
      )
    } finally {
      setSavingSalary(false)
    }
  }

  if (loading) {
    return (
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
          Loading employee details...
        </p>
      </div>
    )
  }

  if (error) {
    return (
      <div>
        <button
          className="btn btn-outline-secondary mb-3"
          onClick={onBack}
        >
          ← Back
        </button>

        <div className="alert alert-danger">
          {error}
        </div>
      </div>
    )
  }

  if (!employee) {
    return (
      <div>
        <button
          className="btn btn-outline-secondary mb-3"
          onClick={onBack}
        >
          ← Back
        </button>

        <div className="alert alert-warning">
          Employee not found.
        </div>
      </div>
    )
  }

  return (
    <div>

      {/* Header */}
      <div className="d-flex justify-content-between align-items-center mb-4">

        <div>
          <h2>Employee Details</h2>

          <p className="text-muted mb-0">
            {employee.employeeCode}
          </p>
        </div>

        <button
          className="btn btn-outline-secondary"
          onClick={onBack}
        >
          ← Back to Employees
        </button>

      </div>

      {/* Employee Information */}
      <div className="card shadow-sm mb-4">

        <div className="card-body">

          <h5 className="mb-4">
            Employee Information
          </h5>

          <div className="row g-3">

            <div className="col-md-4">
              <strong>Employee Code</strong>
              <div>
                {employee.employeeCode}
              </div>
            </div>

            <div className="col-md-4">
              <strong>Name</strong>
              <div>
                {employee.firstName}{' '}
                {employee.lastName}
              </div>
            </div>

            <div className="col-md-4">
              <strong>Email</strong>
              <div>
                {employee.email}
              </div>
            </div>

            <div className="col-md-4">
              <strong>Country</strong>
              <div>
                {employee.country} (
                {employee.countryCode})
              </div>
            </div>

            <div className="col-md-4">
              <strong>Department</strong>
              <div>
                {employee.department}
              </div>
            </div>

            <div className="col-md-4">
              <strong>Job Title</strong>
              <div>
                {employee.jobTitle}
              </div>
            </div>

            <div className="col-md-4">
              <strong>Status</strong>

              <div>
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
              </div>
            </div>

            <div className="col-md-4">
              <strong>Hire Date</strong>

              <div>
                {new Date(
                  employee.hireDate
                ).toLocaleDateString()}
              </div>
            </div>

            <div className="col-md-4">
              <strong>Currency</strong>

              <div>
                {employee.currencyCode}
              </div>
            </div>

          </div>

        </div>
      </div>

      {/* Current Salary */}
      <div className="card shadow-sm mb-4">

        <div className="card-body">

          <h5 className="mb-4">
            Current Salary
          </h5>

          <div className="row g-3">

            <div className="col-md-4">

              <div className="text-muted">
                Base Salary
              </div>

              <h3>
                {employee.currentBaseSalary !== null
                  ? employee.currentBaseSalary.toLocaleString(
                      undefined,
                      {
                        minimumFractionDigits: 2,
                        maximumFractionDigits: 2,
                      }
                    )
                  : 'N/A'}
              </h3>

              <small className="text-muted">
                {employee.currencyCode}
              </small>

            </div>

            <div className="col-md-4">

              <div className="text-muted">
                Bonus
              </div>

              <h3>
                {employee.currentBonus !== null
                  ? employee.currentBonus.toLocaleString(
                      undefined,
                      {
                        minimumFractionDigits: 2,
                        maximumFractionDigits: 2,
                      }
                    )
                  : 'N/A'}
              </h3>

              <small className="text-muted">
                {employee.currencyCode}
              </small>

            </div>

            <div className="col-md-4">

              <div className="text-muted">
                Effective From
              </div>

              <h5 className="mt-2">
                {employee.salaryEffectiveFrom
                  ? new Date(
                      employee.salaryEffectiveFrom
                    ).toLocaleDateString()
                  : 'N/A'}
              </h5>

            </div>

            <div className="col-md-12">

              <div className="text-muted">
                Salary Change Reason
              </div>

              <div>
                {employee.salaryChangeReason ||
                  'N/A'}
              </div>

            </div>

          </div>

        </div>
      </div>

      {/* Update Salary */}
      <div className="card shadow-sm mb-4">

        <div className="card-body">

          <h5 className="mb-4">
            Update Salary
          </h5>

          {salarySuccess && (
            <div className="alert alert-success">
              {salarySuccess}
            </div>
          )}

          {salaryError && (
            <div className="alert alert-danger">
              {salaryError}
            </div>
          )}

          <div className="row g-3">

            <div className="col-md-4">

              <label className="form-label">
                Base Salary
              </label>

              <div className="input-group">

                <input
                  type="number"
                  className="form-control"
                  min="0"
                  step="0.01"
                  value={baseSalary}
                  onChange={(e) =>
                    setBaseSalary(e.target.value)
                  }
                />

                <span className="input-group-text">
                  {employee.currencyCode}
                </span>

              </div>

            </div>

            <div className="col-md-4">

              <label className="form-label">
                Bonus
              </label>

              <div className="input-group">

                <input
                  type="number"
                  className="form-control"
                  min="0"
                  step="0.01"
                  value={bonus}
                  onChange={(e) =>
                    setBonus(e.target.value)
                  }
                />

                <span className="input-group-text">
                  {employee.currencyCode}
                </span>

              </div>

            </div>

            <div className="col-md-4">

              <label className="form-label">
                Effective From
              </label>

              <input
                type="date"
                className="form-control"
                value={effectiveFrom}
                onChange={(e) =>
                  setEffectiveFrom(
                    e.target.value
                  )
                }
              />

            </div>

            <div className="col-md-12">

              <label className="form-label">
                Change Reason
              </label>

              <input
                type="text"
                className="form-control"
                placeholder="e.g. Annual increment"
                value={changeReason}
                onChange={(e) =>
                  setChangeReason(
                    e.target.value
                  )
                }
              />

            </div>

            <div className="col-md-12">

              <button
                type="button"
                className="btn btn-primary"
                onClick={handleSalaryUpdate}
                disabled={savingSalary}
              >
                {savingSalary ? (
                  <>
                    <span
                      className="spinner-border spinner-border-sm me-2"
                      role="status"
                    />
                    Updating...
                  </>
                ) : (
                  'Update Salary'
                )}
              </button>

            </div>

          </div>

        </div>
      </div>

      {/* Salary History */}
      <div className="card shadow-sm mb-4">

        <div className="card-body">

          <div className="d-flex justify-content-between align-items-center mb-4">

            <h5 className="mb-0">
              Salary History
            </h5>

            <span className="text-muted">
              {salaryHistory.length}{' '}
              {salaryHistory.length === 1
                ? 'Record'
                : 'Records'}
            </span>

          </div>

          {salaryHistory.length === 0 ? (

            <div className="alert alert-light border mb-0">
              No salary history available.
            </div>

          ) : (

            <div className="table-responsive">

              <table className="table table-hover align-middle mb-0">

                <thead className="table-light">

                  <tr>
                    <th>Base Salary</th>
                    <th>Bonus</th>
                    <th>Currency</th>
                    <th>Effective From</th>
                    <th>Effective To</th>
                    <th>Reason</th>
                  </tr>

                </thead>

                <tbody>

                  {salaryHistory.map(
                    (salary) => (
                      <tr key={salary.id}>

                        <td>
                          {salary.baseSalary.toLocaleString(
                            undefined,
                            {
                              minimumFractionDigits: 2,
                              maximumFractionDigits: 2,
                            }
                          )}
                        </td>

                        <td>
                          {salary.bonus.toLocaleString(
                            undefined,
                            {
                              minimumFractionDigits: 2,
                              maximumFractionDigits: 2,
                            }
                          )}
                        </td>

                        <td>
                          {salary.currencyCode}
                        </td>

                        <td>
                          {new Date(
                            salary.effectiveFrom
                          ).toLocaleDateString()}
                        </td>

                        <td>
                          {salary.effectiveTo ? (
                            new Date(
                              salary.effectiveTo
                            ).toLocaleDateString()
                          ) : (
                            <span className="badge bg-success">
                              Current
                            </span>
                          )}
                        </td>

                        <td>
                          {salary.changeReason}
                        </td>

                      </tr>
                    )
                  )}

                </tbody>

              </table>

            </div>

          )}

        </div>
      </div>

      {/* Salary Audit */}
      <div className="card shadow-sm mb-4">

        <div className="card-body">

          <div className="d-flex justify-content-between align-items-center mb-4">

            <h5 className="mb-0">
              Salary Audit History
            </h5>

            <span className="text-muted">
              {salaryAudit.length}{' '}
              {salaryAudit.length === 1
                ? 'Change'
                : 'Changes'}
            </span>

          </div>

          {salaryAudit.length === 0 ? (

            <div className="alert alert-light border mb-0">
              No salary changes have been recorded yet.
            </div>

          ) : (

            <div className="table-responsive">

              <table className="table table-hover align-middle mb-0">

                <thead className="table-light">

                  <tr>
                    <th>Previous Salary</th>
                    <th>New Salary</th>
                    <th>Previous Bonus</th>
                    <th>New Bonus</th>
                    <th>Reason</th>
                    <th>Changed At</th>
                    <th>Changed By</th>
                  </tr>

                </thead>

                <tbody>

                  {salaryAudit.map(
                    (audit) => (
                      <tr key={audit.id}>

                        <td>
                          {audit.previousSalary.toLocaleString(
                            undefined,
                            {
                              minimumFractionDigits: 2,
                              maximumFractionDigits: 2,
                            }
                          )}
                        </td>

                        <td>
                          <strong>
                            {audit.newSalary.toLocaleString(
                              undefined,
                              {
                                minimumFractionDigits: 2,
                                maximumFractionDigits: 2,
                              }
                            )}
                          </strong>
                        </td>

                        <td>
                          {audit.previousBonus.toLocaleString(
                            undefined,
                            {
                              minimumFractionDigits: 2,
                              maximumFractionDigits: 2,
                            }
                          )}
                        </td>

                        <td>
                          <strong>
                            {audit.newBonus.toLocaleString(
                              undefined,
                              {
                                minimumFractionDigits: 2,
                                maximumFractionDigits: 2,
                              }
                            )}
                          </strong>
                        </td>

                        <td>
                          {audit.changeReason}
                        </td>

                        <td>
                          {new Date(
                            audit.changedAt
                          ).toLocaleString()}
                        </td>

                        <td>
                          {audit.changedBy}
                        </td>

                      </tr>
                    )
                  )}

                </tbody>

              </table>

            </div>

          )}

        </div>
      </div>

    </div>
  )
}

export default EmployeeDetails
