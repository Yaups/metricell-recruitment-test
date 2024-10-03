import React, { Component } from "react";

class Employee extends Component {
  constructor() {
    super();
    this.state = {
      editMode: false,
      newName: "",
      newValue: "",
    };
  }

  componentWillUnmount() {
    //Fixes warning: Can't perform a React state update on an unmounted component
    //I have decided to use the name as the key for mapping employees, so when the
    //name is edited, this warning comes as a result of modifying the key in a list
    //item in between renders.
    //Ideally we'd have a primary key and this warning would never exist.
    this.setState = (_state, _callback) => {
      return;
    };
  }

  enableEditMode = () => {
    const employee = this.props.employee;

    this.setState({
      editMode: true,
      newName: employee ? employee.name : "",
      newValue: employee ? employee.value : "",
    });
  };

  handleSubmit = async (event) => {
    event.preventDefault();

    if (!this.props.employee) return;

    const creationSuccess = await this.props.editEmployee(
      this.props.employee.name,
      this.state.newName,
      this.state.newValue
    );

    if (creationSuccess)
      this.setState({ editMode: false, name: "", value: "" });
  };

  render() {
    const employee = this.props.employee;

    if (!employee) return null;

    if (this.state.editMode)
      return (
        <>
          <form onSubmit={this.handleSubmit}>
            Name: <br />
            <input
              value={this.state.newName}
              onChange={({ target }) =>
                this.setState({ newName: target.value })
              }
            />
            <br />
            Value: <br />
            <input
              value={this.state.newValue}
              onChange={({ target }) =>
                this.setState({ newValue: target.value })
              }
            />
            <br />
            <button type="submit">Confirm changes</button>{" "}
            <button
              type="button"
              onClick={() => this.setState({ editMode: false })}
            >
              Cancel
            </button>
          </form>
        </>
      );

    return (
      <>
        <li>Name: {employee.name}</li>
        <li>Value: {employee.value}</li>
        <button onClick={this.enableEditMode}>Edit employee</button>{" "}
        <button onClick={() => this.props.deleteEmployee(employee.name)}>
          Delete employee
        </button>
      </>
    );
  }
}

export default Employee;
