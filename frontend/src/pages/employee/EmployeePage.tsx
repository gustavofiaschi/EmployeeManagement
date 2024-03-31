import { SetStateAction, useEffect, useState } from 'react';
import { Input } from '@chakra-ui/react'

import Employee from '../../entities/Employee';
import { getEmployeesData, saveEmployee, deleteEmployee, getYearsOfService } from '../../services/employee-service';
import './EmployeePage.css';

const EmployeePage: React.FC = () => {
    const searches = ['Name', 'JobTitle'];

    const [error, setError] = useState<string>();

    const [criteria, setCriteria] = useState<string>();
    const [termSearch, setTermSearch] = useState<string>();
    const [employees, setEmployees] = useState<Employee[]>();
    const [selectedEmployee, setSelectedEmployee] = useState<Employee | null>();
    const [yearsOfService, setYearsOfService] = useState<number>();

    const [firstNameValue, setFirstNameValue] = useState<string>('');
    const [lastNameValue, setLastNameValue] = useState<string>('');
    const [emailValue, setEmailValue] = useState<string>('');
    const [jobTitleValue, setJobTitleValue] = useState<string>('');
    const [dateOfJoiningValue, setDateOfJoiningValue] = useState<string>('');

    const loadData = async () => {
        const data = await getEmployeesData();
        setEmployees(data);
    }

    useEffect(() => {
        const exec = async () => {
            await loadData();
        }
        exec();
    }, []);

    useEffect(() => {
        const exec = async () => {
            if (selectedEmployee) {
                const data = await getYearsOfService(selectedEmployee?.id);
                setYearsOfService(data);
            }
        }
        exec();
    }, [selectedEmployee]);

    const save = async () => {
        const employee: Employee = {
            id: selectedEmployee?.id ?? 0,
            firstName: firstNameValue,
            lastName: lastNameValue,
            email: emailValue,
            jobTitle: jobTitleValue,
            dateOfJoining: dateOfJoiningValue
        };
        await saveEmployee(employee);
        
        onNew();
        await loadData();
    }

    const deleteOp = async () => {
        await deleteEmployee(selectedEmployee?.id);
        onNew();
        await loadData();
    }

    const onChangeSelect = (event: { target: { value: SetStateAction<string | undefined>; }; }) => {
        setCriteria(event.target.value);
    }

    const onChange = (event: { target: { value: SetStateAction<string | undefined>; }; }) => {
        setTermSearch(event.target.value);
    }

    const onClick = async () => {
        const data = await getEmployeesData();

        if (termSearch === '' || termSearch === undefined) {
            setEmployees(data);
            return;
        }

        let filterEmployees;

        if (criteria === 'Name') {
            filterEmployees = data?.filter(e => e.firstName.toLowerCase().includes(termSearch.toLowerCase()) || e.lastName.toLowerCase().includes(termSearch.toLowerCase()));
        } else if (criteria === 'JobTitle') {
            filterEmployees = data?.filter(e => e.jobTitle.toLowerCase().includes(termSearch.toLowerCase()));
        }

        setEmployees(filterEmployees);
    }

    const selectEmployee = (employee: Employee) => {
        setFirstNameValue(employee.firstName);
        setLastNameValue(employee.lastName);
        setEmailValue(employee.email);
        setJobTitleValue(employee.jobTitle);
        setDateOfJoiningValue(employee.dateOfJoining);

        setSelectedEmployee(employee);
    }

    const onNew = () => {
        setFirstNameValue('');
        setLastNameValue('');
        setEmailValue('');
        setJobTitleValue('');
        setDateOfJoiningValue('');

        setSelectedEmployee(null);
    }

    const form =
        <section className="section">
            <div className="grid">
                <div className="row">
                    <div className="grid">
                        <label className="labelField">First Name:</label>
                        <label className="labelField">Last Name:</label>
                        <label className="labelField">Email:</label>
                        <label className="labelField">Job Title:</label>
                        <label className="labelField">Date of Joining:</label>
                    </div>
                    <div className="grid">
                        <Input id='firstname' value={firstNameValue} onChange={(e) => setFirstNameValue(e.target.value)} />
                        <Input id="lastname" value={lastNameValue} onChange={(e) => setLastNameValue(e.target.value)} />
                        <Input id="email" value={emailValue} onChange={(e) => setEmailValue(e.target.value)} />
                        <Input id="jobtitle" value={jobTitleValue} onChange={(e) => setJobTitleValue(e.target.value)} />
                        <Input id="dateofjoining" type="datetime-local" value={dateOfJoiningValue} onChange={(e) => setDateOfJoiningValue(e.target.value)} />
                    </div>
                    <div className="grid">
                        <label className="labelField">Years of Service: </label>
                        <label className="labelField"><b>{yearsOfService}</b></label>
                    </div>
                </div>                             
                <div className="row">
                    <button className="button" onClick={onNew}>New</button>
                    <button className="button" onClick={save}>Save</button>
                    <button className="button" onClick={deleteOp}>Delete</button>
                </div>
            </div>
        </section>;

    const filters =
        <section>
            <div className="row">
                <h3>Filter</h3>
                <label>By:</label>
                <select onChange={onChangeSelect}>
                    <option>Select</option>
                    {searches.map(s =>
                        <option key={s} value={s}>{s}</option>
                    )}
                </select>
                <label>Value:</label>
                <input id="searchValue" type="text" onChange={onChange} />
                <button className="button" onClick={onClick}>Search</button>
            </div>            
        </section>;

    const tableContents =
        <table className="table">
            <thead>
                <tr>
                    <th>Name</th>
                    <th>Email</th>
                    <th>Job Title</th>
                    <th>Date of Joining</th>
                </tr>
            </thead>
            <tbody>
                {employees?.map(employee =>
                    <tr className={employee.id === selectedEmployee?.id ? "selectedTr" : "" } key={employee.id} onClick={() => selectEmployee(employee)}>
                        <td>{employee.firstName} {employee.lastName}</td>
                        <td>{employee.email}</td>
                        <td>{employee.jobTitle}</td>
                        <td>{employee?.dateOfJoining.substring(0, employee?.dateOfJoining.indexOf('T'))}</td>
                    </tr>
                )}
            </tbody>
        </table>;

    return (
        <div>
            <h1 id="tabelLabel">Employees Management</h1>
            {form}
            {filters}
            {tableContents}
        </div>
    );
}

export default EmployeePage;