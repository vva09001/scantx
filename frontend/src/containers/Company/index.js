import React, { Component } from "react";
import { connect } from "react-redux";
import Form from "./Form";
import { CircularProgress } from "components/uielements/progress";
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
  TableBody,
  TableCell,
  TableHead,
  TableRow
} from "components/uielements/table";
import { companyActions } from "redux/actions";
import { permission } from "helpers/user";
import _ from "lodash";
import "styles/style.css";

class Company extends Component {
  constructor(props) {
    super(props);
    this.state = {
      loading: true,
      toggle: false,
      delete: false,
      params: {},
      editAble: false,
      selected: [],
      selectAlert: false,
      assignCid: null,
      toggleAssign: false,
      page: 0,
      rowsPerPage: 20
    };
  }

  componentDidMount() {
    this.props.get(this.onSuccess, this.onSuccess);
  }

  onChange = params => {
    this.setState({
      params
    });
  };

  onSuccess = () => {
    this.setState({
      loading: false
    });
  };

  onToggleForm = status => {
    this.setState({
      toggle: status,
      editAble: false,
      params: {}
    });
  };

  onToggleEditForm = (status, item) => {
    if (status === true) {
      this.setState({
        toggle: status,
        editAble: true,
        params: item
      });
    } else {
      this.setState({
        toggle: status
      });
    }
  };

  onToggleDelete = status => {
    if (this.state.selected.length <= 0) {
      this.setState({
        selectAlert: status
      });
    } else {
      this.setState({
        delete: status
      });
    }
  };

  onSubmit = (params, success, fail) => {
    if (this.state.editAble) {
      this.props.edit(params, success, fail);
    } else {
      this.props.add(params, success, fail);
    }
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
    return this.props.companies
      .slice(page * rowsPerPage, page * rowsPerPage + rowsPerPage)
      .map(item => (
        <TableRow key={item.cid}>
          <TableCell padding="checkbox">
            <Checkbox
              onChange={() => this.onSelected(item.cid)}
              checked={_.includes(this.state.selected, item.cid)}
            />
          </TableCell>
          <TableCell>{item.name}</TableCell>
          <TableCell>{item.address}</TableCell>
          <TableCell>{item.status}</TableCell>
          <TableCell>
            {_.indexOf(permission.user.edit, profile.roleID) !== -1 && (
              <span>
                <EditIcon
                  className="actions"
                  onClick={() => this.onToggleEditForm(true, item)}
                />
              </span>
            )}
          </TableCell>
        </TableRow>
      ));
  };

  onToggleAssign = (status, assignCid) => {
    this.setState({
      toggleAssign: status,
      assignCid: assignCid
    });
  };

  onAssign = (params, success, fail) => {
    this.props.assign(this.state.assignCid, params, success, fail);
  };

  onSelected = id => {
    const { selected } = this.state;

    if (_.includes(selected, id)) {
      this.setState({
        selected: _.filter(selected, item => {
          return item !== id;
        })
      });
    } else {
      this.setState({
        selected: [...selected, id]
      });
    }
  };

  onSelectedAll = (status = false) => {
    const { selected } = this.state;
    const { companies } = this.props;

    if (selected.length === companies.length || status) {
      this.setState({
        selected: []
      });
    } else {
      this.setState({
        selected: _.map(companies, item => {
          return item.cid;
        })
      });
    }
  };

  onToggleSelectAlert = status => {
    this.setState({ selectAlert: status });
  };

  render() {
    const { profile } = this.props;
    if (this.state.loading) {
      return <CircularProgress />;
    }
    return (
      <LayoutWrapper>
        <FullColumn>
          <Papersheet title="Companies">
            <Table>
              <TableHead>
                <TableRow>
                  <TableCell padding="checkbox">
                    <Checkbox
                      onChange={() => this.onSelectedAll()}
                      checked={
                        this.state.selected.length &&
                        this.state.selected.length ===
                          this.props.companies.length
                      }
                    />
                  </TableCell>
                  <TableCell>Name</TableCell>
                  <TableCell>Address</TableCell>
                  <TableCell>Remark</TableCell>
                  <TableCell>Actions</TableCell>
                </TableRow>
              </TableHead>
              <TableBody>{this.renderData()}</TableBody>
            </Table>
            <TablePagination
              component="div"
              rowsPerPageOptions={[10, 20, 30]}
              count={this.props.companies.length}
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
              {_.indexOf(permission.company.add, profile.roleID) !== -1 && (
                <Button
                  className="buttonStyles"
                  variant="contained"
                  color="primary"
                  size="small"
                  onClick={() => this.onToggleForm(true)}
                >
                  Add new company
                </Button>
              )}
              {_.indexOf(permission.company.delete, profile.roleID) !== -1 && (
                <Button
                  className="buttonStyles"
                  variant="contained"
                  color="primary"
                  size="small"
                  onClick={() => this.onToggleDelete(true)}
                >
                  Delete selected
                </Button>
              )}
            </Grid>
          </Papersheet>
          <Form
            onToggle={this.onToggleForm}
            status={this.state.toggle}
            params={this.state.params}
            onChange={this.onChange}
            onSubmit={this.onSubmit}
            remove={this.onSelectedAll}
          />
          {/* Assign Form */}
          {/* <AssignForm
            onToggle={this.onToggleAssign}
            onSubmit={this.onAssign}
            status={this.state.toggleAssign}
          /> */}
          <DeleteAlert
            status={this.state.delete}
            selected={this.state.selected}
            onClose={this.onToggleDelete}
            remove={this.onSelectedAll}
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
    companies: state.Company.list,
    profile: state.Auth.profile
  };
};

const mapDispatchToProps = {
  get: companyActions.get,
  add: companyActions.add,
  edit: companyActions.edit,
  assign: companyActions.assign
};
export default connect(
  mapStateToProps,
  mapDispatchToProps
)(Company);
