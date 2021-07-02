import React, { Component } from 'react';
import { HubConnectionBuilder } from '@aspnet/signalr';
import { AuthContext } from '../AuthContext';


class Home extends Component {
  state =
    {
      tasks: [],
      taskname: '',
      connection: null
    }

  componentDidMount = async () => {

    const connection = new HubConnectionBuilder().withUrl("/tasks").build();
    this.setState({ connection });

    await connection.start();

    connection.invoke("sendtasks");
    connection.on("showtasks", tasks => this.setState({ tasks })   );
    
    console.log(this.state.tasks)
  }

  onTaskChange = e => {
    this.setState({ taskname: e.target.value });
  }

  onClickAddTask = async () => {
    const { connection } = this.state;
    await connection.invoke("addtask", { description: this.state.taskname });
    this.setState({ taskname: '' })
  }

  onCompleteTask = async (t) => {
    const { connection } = this.state;
    await connection.invoke("closetask", { id: t.id });
    this.setState({ taskname: '' });
  }

  onAssignTask = (t) => {
    const { connection } = this.state;
    connection.invoke("assigntask", { id: t.id, description: t.description, username: this.state.username });
  }

  onUnassignTask = (t) => {
    const { connection } = this.state;
    connection.invoke("unassigntask", { id: t.id });

  }

  buildButton = (t, user) => {

   if (user) {
    if (t.isAssigned && t.userId !== user.id) {
      return <button disabled className="btn btn-warning">{t.userName} has this one</button>
    }

    if (t.isAssigned && t.userId === user.id) {
      return (
        <>
          <button className="btn btn-success" onClick={() => this.onCompleteTask(t)} >I'm done</button>
          <button className="btn btn-warning" onClick={() => this.onUnassignTask(t)}>Unassign</button>
        </>
      )
    }
  }
    return <button disabled={!user} className="btn btn-info" onClick={() => this.onAssignTask(t)}>I'll take this one! </button>
   
  }

  render() {

    return (
      <AuthContext.Consumer>
        {/* {({ user }) => { */}
        {value => {
          const { user } = value;
          return (

            <div>
              <h2 style={{ textAlign: "center" }}>Tasks</h2>
              {!user && <h3 style={{color: 'red'}}>You must login to add or manage tasks. </h3> }
              
              <div className="row" style={{marginTop: 20}}>
                <div className="col-md-6" >
                  <input value={this.state.taskname} type="text" className="form-control" onChange={this.onTaskChange} placeholder="Task Name"></input>
                </div>
                <div className="col-md-2">
                  <button disabled={!user ||!this.state.taskname.trim()} className="btn btn-primary" onClick={this.onClickAddTask}>Add Task</button>
                </div>
              </div>

              <table className="table table-hover table-striped table-bordered" style={{ marginTop: 30 }}>
                <thead>
                  <tr>
                    <th>Task Name</th>
                    <th>Status</th>
                  </tr>
                </thead>
                <tbody>

                  {this.state.tasks.map(t =>
                    <tr key={t.id}>
                      <td>{t.description}</td>
                      <td> {this.buildButton(t, user)} </td>
                    </tr>
                  )}
                </tbody>
              </table>
            </div>

          )
        }}

      </AuthContext.Consumer>
    );
  }
}

export default Home;