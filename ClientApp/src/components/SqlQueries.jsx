import React, { Component } from "react";
import employeeService from "../services/employeeService";

class SqlQueries extends Component {
  constructor() {
    super();
    this.state = { sumQueryResult: null };
  }

  getSumQueryResult = async () => {
    try {
      const sumQueryResult = await employeeService.getSumQueryResult();
      console.log(sumQueryResult);
      this.setState({ sumQueryResult });
    } catch (error) {
      alert(error.message);
    }
  };

  executeIncrementQuery = async () => {
    try {
      await employeeService.executeIncrementQuery();
      await this.props.refetchEmployees();
    } catch (error) {
      alert(error.message);
    }
  };

  render() {
    const sumQueryResult = this.state.sumQueryResult;

    return (
      <div>
        <h2>Pre-made SQL queries to execute:</h2>
        <p>
          Pressing the button below will do the following: Increment the field
          Value by 1 where the field Name begins with E and by 10 where Name
          begins with G and all others by 100.
        </p>
        <button onClick={this.executeIncrementQuery}>Execute</button>
        <br />
        <br />
        <p>
          Pressing the button below will do the following: List the sum of all
          Values for all Names that begin with A, B or C but only present the
          data where the summed values are greater than or equal to 11171
        </p>
        <button onClick={this.getSumQueryResult}>Execute</button>
        <br />
        {sumQueryResult && (
          <>
            <h3>Sum query result: </h3>
            {sumQueryResult.sumOfA !== -1 && (
              <p>
                Sum of values for names beginning with A:{" "}
                {sumQueryResult.sumOfA}
              </p>
            )}
            {sumQueryResult.sumOfB !== -1 && (
              <p>
                Sum of values for names beginning with B:{" "}
                {sumQueryResult.sumOfB}
              </p>
            )}
            {sumQueryResult.sumOfC !== -1 && (
              <p>
                Sum of values for names beginning with C:{" "}
                {sumQueryResult.sumOfC}
              </p>
            )}
            {sumQueryResult.sumOfA === -1 &&
              sumQueryResult.sumOfB === -1 &&
              sumQueryResult.sumOfC === -1 && <p>No data to show!</p>}
          </>
        )}
      </div>
    );
  }
}

export default SqlQueries;
