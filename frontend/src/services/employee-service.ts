import Employee from "../entities/Employee";

const URL_BASE = "https://localhost:7290";

export const getEmployeesData = async (): Promise<Employee[]> => {
    const response = await fetch(`${URL_BASE}/Employee`);
    const data = await response?.json();
    return data;
}

export const saveEmployee = async (selectedEmployee: Employee | undefined | null) => {
    const requestOptions = {
        method: selectedEmployee?.id != 0 ? 'PUT' : 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(selectedEmployee)
    };

    await fetch(`${URL_BASE}/Employee`, requestOptions);
}

export const deleteEmployee = async (id: number | undefined | null) => {
    if (id) {
        const requestOptions = {
            method: 'DELETE',
            headers: { 'Content-Type': 'application/json' }
        };

        await fetch(`${URL_BASE}/Employee?Id=${id}`, requestOptions);
    }
}

export const getYearsOfService = async (employeeId: number | null | undefined): Promise<number> => {
    const response = await fetch(`${URL_BASE}/Employee/GetEmployeesYearsOfService/${employeeId}`);
    const data = await response?.json();
    return data;
}