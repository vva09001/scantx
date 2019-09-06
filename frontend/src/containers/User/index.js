import React, { Component } from "react";
import { connect } from "react-redux";
import { CircularProgress } from "components/uielements/progress";
import Form from "./Form";
import EditForm from "./EditForm";
import DeleteAlert from "./Alert";
import LayoutWrapper from "components/utility/layoutWrapper";
import Papersheet from "components/utility/papersheet";
import { FullColumn } from "components/utility/rowColumn";
import Button from "components/uielements/button/index.js";
import Grid from "components/uielements/grid";
import Table from "components/uielements/table";
import Checkbox from "components/uielements/checkbox";
import Link from "@material-ui/core/Link";
import { TableBody, TableCell, TableRow } from "components/uielements/table";
import { userActions, companyActions } from "redux/actions";
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
      toggleEdit: false,
      editAlert: false,
      deleteMulti: false,
      multiId: [],
      params: {}
    };
  }
  componentDidMount() {
    this.props.getUser(this.onSuccess, this.onSuccess);
    this.props.getCompanies(this.onSuccess, this.onSuccess);
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
    this.setState({
      deleteMulti: status
    });
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
          <TableCell>{role(item.roleId)}</TableCell>
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
              {/* <Button
                className="buttonStyles"
                variant="contained"
                color="primary"
              >
                Edit selected
              </Button> */}
              <Button
                className="buttonStyles"
                variant="contained"
                color="primary"
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
            companies={this.props.companies}
          />
          {/* Edit Form */}
          <EditForm
            onToggle={this.onToggleEditForm}
            onSubmit={this.edit}
            status={this.state.toggleEdit}
            params={this.state.params}
            companies={this.props.companies}
          />
          {/* Delete Multi Alert */}
          <DeleteAlert
            status={this.state.deleteMulti}
            onSubmit={this.deleteMulti}
            onClose={this.onToggleDeleteMulti}
          />
        </FullColumn>
      </LayoutWrapper>
    );
  }
}
const mapStateToProps = state => {
  return {
    users: state.User.list,
    companies: state.Company.list
  };
};

const mapDispatchToProps = {
  getUser: userActions.getUser,
  addUser: userActions.addUser,
  editUser: userActions.editUser,
  deleteMultiUser: userActions.deleteMultiUser,
  getCompanies: companyActions.get
};
export default connect(
  mapStateToProps,
  mapDispatchToProps
)(User);
