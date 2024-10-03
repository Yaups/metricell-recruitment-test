import React, { Component } from "react";
import employeeService from "../services/employeeService";
import Employee from "./Employee";
import NewEmployeeForm from "./NewEmployeeForm";
import SqlQueries from "./SqlQueries";

class EmployeeList extends Component {
  constructor() {
    super();
    this.state = { employees: null };
  }

  componentDidMount = async () => {
    await this.refetchEmployees();
  };

  refetchEmployees = async () => {
    const employees = await employeeService.getAll();
    this.setState({ employees });
  };

  createEmployee = async (name, value) => {
    if (!name || !value) return;

    const newEmployee = { name, value };

    try {
      const createdEmployee = await employeeService.create(newEmployee);
      if (!createdEmployee) return alert("Employee could not be created");
      this.setState({ employees: [...this.state.employees, createdEmployee] });
      return true;
    } catch (error) {
      alert(error.message);
      return false;
    }
  };

  editEmployee = async (name, updatedName, updatedValue) => {
    if (!name || !updatedName || !updatedValue) return;

    if (!window.confirm("Are you sure you would like to update this employee?"))
      return;

    const updatedEmployee = { name: updatedName, value: updatedValue };

    try {
      await employeeService.findByNameAndUpdate(name, updatedEmployee);

      const employeesWithUpdated = this.state.employees.map((employee) =>
        employee.name === name ? updatedEmployee : employee
      );

      this.setState({ employees: employeesWithUpdated });

      return true;
    } catch (error) {
      alert(error.message);
      return false;
    }
  };

  deleteEmployee = async (name) => {
    if (!name) return;

    if (!window.confirm("Are you sure you would like to delete this employee?"))
      return;

    try {
      await employeeService.findByNameAndDelete(name);
      this.setState({
        employees: this.state.employees.filter((e) => e.name !== name),
      });
    } catch (error) {
      alert(error.message);
    }
  };

  render() {
    return (
      <>
        <SqlQueries refetchEmployees={this.refetchEmployees} />
        <br />
        <hr />
        <NewEmployeeForm createEmployee={this.createEmployee} />
        <br />
        <hr />
        <h2>Employees:</h2>
        <ul>
          {this.state.employees
            ?.toSorted((a, b) => a.name.localeCompare(b.name))
            .map((employee) => {
              return (
                <div key={employee.name}>
                  <Employee
                    employee={employee}
                    deleteEmployee={this.deleteEmployee}
                    editEmployee={this.editEmployee}
                  />
                  <br />
                  <br />
                </div>
              );
            })}
        </ul>
      </>
    );
  }
}

export default EmployeeList;
