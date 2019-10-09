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
import EditIcon from "@material-ui/icons/Edit";
import TablePagination from "@material-ui/core/TablePagination";
import {
  TableHead,
  TableBody,
  TableCell,
  TableRow
} from "components/uielements/table";
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
      params: {},
      page: 0,
      rowsPerPage: 20
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

  onSelectedAll = (status = false) => {
    const { multiId } = this.state;
    const { users } = this.props;

    if (multiId.length === users.length || status) {
      this.setState({
        multiId: []
      });
    } else {
      this.setState({
        multiId: _.map(users, item => {
          return item.id;
        })
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

  handleChangePage = (event, newPage) => {
    this.setState({
      page: newPage
    });
  };

  handleChangeRowsPerPage = event => {
    this.setState({
      rowsPerPage: +event.target.value,
      page: 0
    });
  };

  renderData = () => {
    const { profile } = this.props;
    const { page, rowsPerPage } = this.state;
    return this.props.users
      .slice(page * rowsPerPage, page * rowsPerPage + rowsPerPage)
      .map(item => {
        return (
          <TableRow key={item.id}>
            <TableCell padding="checkbox">
              <Checkbox
                onChange={e => this.handleCheck(e, item.id)}
                checked={_.includes(this.state.multiId, item.id)}
              />
            </TableCell>
            <TableCell>{item.userName}</TableCell>
            <TableCell>
              {item.givenName} {item.familyName}
            </TableCell>
            <TableCell>{item.email}</TableCell>
            <TableCell>{role(item.roleID)}</TableCell>
            <TableCell>
              {_.indexOf(permission.user.edit, profile.roleID) !== -1 && (
                <span
                  className="actions"
                  onClick={() => this.onToggleEditForm(true, item)}
                >
                  <EditIcon />
                </span>
              )}
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
              <TableHead>
                <TableRow>
                  <TableCell padding="checkbox">
                    <Checkbox
                      onChange={() => this.onSelectedAll()}
                      checked={
                        this.state.multiId.length &&
                        this.state.multiId.length === this.props.users.length
                      }
                    />
                  </TableCell>
                  <TableCell>Username</TableCell>
                  <TableCell>Name</TableCell>
                  <TableCell>Email</TableCell>
                  <TableCell>Role</TableCell>
                  <TableCell>Actions</TableCell>
                </TableRow>
              </TableHead>
              <TableBody>{this.renderData()}</TableBody>
            </Table>
            <TablePagination
              component="div"
              rowsPerPageOptions={[10, 20, 30]}
              count={this.props.users.length}
              rowsPerPage={this.state.rowsPerPage}
              page={this.state.page}
              backIconButtonProps={{
                "aria-label": "previous page"
              }}
              nextIconButtonProps={{
                "aria-label": "next page"
              }}
              onChangePage={this.handleChangePage}
              onChangeRowsPerPage={this.handleChangeRowsPerPage}
            />
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
              {_.indexOf(permission.user.delete, profile.roleID) !== -1 && (
                <Button
                  className="buttonStyles"
                  variant="contained"
                  color="primary"
                  size="small"
                  onClick={() => this.onToggleDeleteMulti(true)}
                >
                  Delete selected
                </Button>
              )}
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
