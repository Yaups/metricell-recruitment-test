import React, { Component } from "react";

class NewEmployeeForm extends Component {
  constructor() {
    super();
    this.state = { name: "", value: "" };
  }

  handleSubmit = async (event) => {
    event.preventDefault();
    const creationSuccess = await this.props.createEmployee(
      this.state.name,
      this.state.value
    );
    if (creationSuccess) this.setState({ name: "", value: "" });
  };

  render() {
    return (
      <>
        <h2>Add a new employee:</h2>
        <form onSubmit={this.handleSubmit}>
          Name:{" "}
          <input
            value={this.state.name}
            onChange={({ target }) => this.setState({ name: target.value })}
          />
          <br />
          <br />
          Value:{" "}
          <input
            value={this.state.value}
            onChange={({ target }) => this.setState({ value: target.value })}
          />
          <br />
          <br />
          <button type="submit">Add employee</button>
        </form>
      </>
    );
  }
}

export default NewEmployeeForm;
