const getAll = async () => {
  const response = await fetch("employees");

  if (!response.ok) {
    const errorMessage = await response.text();
    return Promise.reject(
      new Error("Get all employees failed: " + errorMessage ?? "Unknown error")
    );
  }

  return response.json();
};

const getByName = async (name) => {
  const response = await fetch(`list/${name}`);

  if (!response.ok) {
    const errorMessage = await response.text();
    return Promise.reject(
      new Error("Get employee failed: " + errorMessage ?? "Unknown error")
    );
  }

  return response.json();
};

const create = async (newEmployee) => {
  const response = await fetch("list", {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify(newEmployee),
  });

  if (!response.ok) {
    const errorMessage = await response.text();
    return Promise.reject(
      new Error(
        "Create new employee failed: " + errorMessage ?? "Unknown error"
      )
    );
  }

  return response.json();
};

const findByNameAndUpdate = async (name, updatedEmployee) => {
  const response = await fetch(`list/${name}`, {
    method: "PUT",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify(updatedEmployee),
  });

  if (!response.ok) {
    const errorMessage = await response.text();
    return Promise.reject(
      new Error("Update employee failed: " + errorMessage ?? "Unknown error")
    );
  }
};

const findByNameAndDelete = async (name) => {
  const response = await fetch(`list/${name}`, {
    method: "DELETE",
  });

  if (!response.ok) {
    const errorMessage = await response.text();
    return Promise.reject(
      new Error("Delete employee failed: " + errorMessage ?? "Unknown error")
    );
  }
};

const executeIncrementQuery = async () => {
  const response = await fetch("list/increment", {
    method: "PUT",
  });

  if (!response.ok) {
    const errorMessage = await response.text();
    return Promise.reject(
      new Error("Error executing query: " + errorMessage ?? "Unknown error")
    );
  }
};

const getSumQueryResult = async () => {
  const response = await fetch("list/sum");

  if (!response.ok) {
    const errorMessage = await response.text();
    return Promise.reject(
      new Error("Error executing query: " + errorMessage ?? "Unknown error")
    );
  }

  return response.json();
};

export default {
  getAll,
  getByName,
  create,
  findByNameAndUpdate,
  findByNameAndDelete,
  executeIncrementQuery,
  getSumQueryResult,
};
