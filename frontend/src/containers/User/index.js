import React, { Component } from "react";
import { connect } from "react-redux";
import { CircularProgress } from "components/uielements/progress";
import Form from "./Form";
import LayoutWrapper from "components/utility/layoutWrapper";
import Papersheet from "components/utility/papersheet";
import { FullColumn } from "components/utility/rowColumn";
import Button from "components/uielements/button/index.js";
import Grid from "components/uielements/grid";
import Table from "components/uielements/table";
import Checkbox from "components/uielements/checkbox";
import { TableBody, TableCell, TableRow } from "components/uielements/table";
import { userActions } from "redux/actions";
import _ from "lodash";
import "styles/style.css";

const role = roleId => {
  if (roleId === "1") {
    return "superadmin";
  } else if (roleId === "2") {
    return "admin";
  } else if (roleId === "3") {
    return "user";
  } else if (roleId === "4") {
    return "reader";
  } else {
    return "No Role";
  }
};

class User extends Component {
  constructor(props) {
    super(props);
    this.state = {
      loading: true,
      toggle: false,
      params: {}
    };
  }
  componentDidMount() {
    this.props.getUser({}, this.onSuccess, this.onSuccess);
  }
  onSuccess = () => {
    this.setState({
      loading: false
    });
  };

  onToggleForm = status => {
    this.setState({
      toggle: status
    });
  };

  add = (params, success, fail) => {
    this.props.addUser(params, success, fail);
    this.setState({
      toggle: false
    });
  };

  renderData = () => {
    return _.map(this.props.users, item => {
      return (
        <TableRow key={item.id}>
          <TableCell padding="checkbox">
            <Checkbox />
          </TableCell>
          <TableCell>
            {item.givenName} {item.familyName}
          </TableCell>
          <TableCell>{item.email}</TableCell>
          <TableCell>{role(item.roleId)}</TableCell>
        </TableRow>
      );
    });
  };
  render() {
    if (this.state.loading) {
      return <CircularProgress />;
    }
    return (
      <LayoutWrapper>
        <FullColumn>
          <Papersheet title="Users of company CSBG">
            <Table>
              <TableBody>{this.renderData()}</TableBody>
            </Table>
            <Grid
              container
              direction="row"
              justify="center"
              alignItems="center"
            >
              <Button
                className="buttonStyles"
                variant="contained"
                color="primary"
                onClick={() => this.onToggleForm(true)}
              >
                Add new user
              </Button>
              <Button
                className="buttonStyles"
                variant="contained"
                color="primary"
              >
                Edit selected
              </Button>
              <Button
                className="buttonStyles"
                variant="contained"
                color="primary"
              >
                Delete selected
              </Button>
            </Grid>
          </Papersheet>
          {/* Add Form */}
          <Form
            onToggle={this.onToggleForm}
            onSubmit={this.add}
            status={this.state.toggle}
          />
        </FullColumn>
      </LayoutWrapper>
    );
  }
}
const mapSateToProps = state => {
  return {
    users: state.User.list
  };
};

const mapDispatchToProps = {
  getUser: userActions.getUser,
  addUser: userActions.addUser
};
export default connect(
  mapSateToProps,
  mapDispatchToProps
)(User);
