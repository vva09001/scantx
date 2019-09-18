import React, { Component } from "react";
import { connect } from "react-redux";
import { CircularProgress } from "components/uielements/progress";
import Form from "./Form";
import EditForm from "./EditForm";
import DeleteAlert from "./Alert";
import SelectAlert from "./SelectAlert";
import LayoutWrapper from "components/utility/layoutWrapper";
import Papersheet from "components/utility/papersheet";
import { FullColumn } from "components/utility/rowColumn";
import Button from "components/uielements/button/index.js";
import Grid from "components/uielements/grid";
import Table from "components/uielements/table";
import Checkbox from "components/uielements/checkbox";
import Link from "@material-ui/core/Link";
import { TableBody, TableCell, TableRow } from "components/uielements/table";
import { userActions } from "redux/actions";
import { permission, role, generatePassword } from "helpers/user";
import _ from "lodash";
import "styles/style.css";

class User extends Component {
  constructor(props) {
    super(props);
    this.state = {
      loading: true,
      toggle: false,
      toggleEdit: false,
      editAlert: false,
      deleteMulti: false,
      multiId: [],
      selectAlert: false,
      password: "",
      params: {}
    };
  }
  componentDidMount() {
    this.props.getUser(this.onSuccess, this.onSuccess);
  }
  onSuccess = () => {
    this.setState({
      loading: false
    });
  };

  onToggleForm = status => {
    if (status === true) {
      this.setState({
        toggle: status,
        password: generatePassword()
      });
    } else {
      this.setState({
        toggle: status
      });
    }
  };

  onToggleEditForm = (status, item) => {
    if (status === true) {
      this.setState({
        toggleEdit: status,
        params: item
      });
    } else {
      this.setState({
        toggleEdit: status
      });
    }
  };

  onToggleDelete = (status, deleteId) => {
    this.setState({
      delete: status,
      deleteId: deleteId
    });
  };

  onToggleDeleteMulti = status => {
    if (this.state.multiId.length <= 0) {
      this.setState({
        selectAlert: status
      });
    } else {
      this.setState({
        deleteMulti: status
      });
    }
  };

  onToggleSelectAlert = status => {
    this.setState({ selectAlert: status });
  };

  handleCheck = (event, id) => {
    if (event.target.checked) {
      this.setState({
        multiId: [...this.state.multiId, id]
      });
    } else {
      this.setState({
        multiId: this.state.multiId.filter(item => item !== id)
      });
    }
  };

  edit = (params, success, fail) => {
    this.props.editUser(params, success, fail);
    this.setState({
      toggleEdit: false,
      params: {}
    });
  };

  add = (params, success, fail) => {
    this.props.addUser(params, success, fail);
    this.setState({
      toggle: false
    });
  };

  deleteMulti = () => {
    this.props.deleteMultiUser(
      this.state.multiId,
      this.onSuccess,
      this.onSuccess
    );
    this.setState({
      deleteMulti: false,
      multiId: []
    });
  };

  renderData = () => {
    return _.map(this.props.users, item => {
      return (
        <TableRow key={item.id}>
          <TableCell padding="checkbox">
            <Checkbox onChange={e => this.handleCheck(e, item.id)} />
          </TableCell>
          <TableCell>
            {item.givenName} {item.familyName}
          </TableCell>
          <TableCell>{item.email}</TableCell>
          <TableCell>{role(item.roleID)}</TableCell>
          <TableCell>
            <Link
              component="button"
              variant="body2"
              onClick={() => this.onToggleEditForm(true, item)}
            >
              Edit
            </Link>
          </TableCell>
        </TableRow>
      );
    });
  };
  render() {
    const { profile } = this.props;

    if (this.state.loading) {
      return <CircularProgress />;
    }
    return (
      <LayoutWrapper>
        <FullColumn>
          <Papersheet
            title={
              _.indexOf(permission.user.getAll, profile.roleID) !== -1
                ? "Users of all companies"
                : "Users of company " + profile.companyName
            }
          >
            <Table>
              <TableBody>{this.renderData()}</TableBody>
            </Table>
            <Grid
              container
              direction="row"
              justify="center"
              alignItems="center"
            >
              {_.indexOf(permission.user.add, profile.roleID) !== -1 && (
                <Button
                  className="buttonStyles"
                  variant="contained"
                  color="primary"
                  size="small"
                  onClick={() => this.onToggleForm(true)}
                >
                  Add new user
                </Button>
              )}
              <Button
                className="buttonStyles"
                variant="contained"
                color="primary"
                size="small"
                onClick={() => this.onToggleDeleteMulti(true)}
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
            password={this.state.password}
          />
          {/* Edit Form */}
          <EditForm
            onToggle={this.onToggleEditForm}
            onSubmit={this.edit}
            status={this.state.toggleEdit}
            params={this.state.params}
          />
          {/* Delete Multi Alert */}
          <DeleteAlert
            status={this.state.deleteMulti}
            onSubmit={this.deleteMulti}
            onClose={this.onToggleDeleteMulti}
          />
          {/* Select Alert */}
          <SelectAlert
            status={this.state.selectAlert}
            onClose={this.onToggleSelectAlert}
          />
        </FullColumn>
      </LayoutWrapper>
    );
  }
}
const mapStateToProps = state => {
  return {
    users: state.User.list,
    profile: state.Auth.profile
  };
};

const mapDispatchToProps = {
  getUser: userActions.getUser,
  addUser: userActions.addUser,
  editUser: userActions.editUser,
  deleteMultiUser: userActions.deleteMultiUser
};
export default connect(
  mapStateToProps,
  mapDispatchToProps
)(User);
