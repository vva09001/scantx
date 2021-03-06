import React from "react";
import { connect } from "react-redux";
import { TextField } from "@material-ui/core";
import Dialog, {
  DialogActions,
  DialogContent,
  DialogTitle
} from "components/uielements/dialogs";
import Button from "components/uielements/button";
import NativeSelect from "@material-ui/core/NativeSelect";
import FormControl from "@material-ui/core/FormControl";
import FormControlLabel from "@material-ui/core/FormControlLabel";
import FormLabel from "@material-ui/core/FormLabel";
import InputLabel from "@material-ui/core/InputLabel";
import Checkbox from "components/uielements/checkbox";
import { CircularProgress } from "components/uielements/progress";
import { permission } from "helpers/user";
import _ from "lodash";

class Form extends React.Component {
  constructor(props) {
    super(props);
    this.state = {
      loading: false,
      error: {
        status: false,
        message: ""
      },
      readerRole: false,
      userRole: false,
      adminRole: false,
      superadminRole: false,
      params: {
        userName: "",
        givenName: "",
        familyName: "",
        typeOfAccount: "Commercial",
        roleID: "",
        password: "",
        email: "",
        contactByEmail: false,
        encryptionActive: false
      }
    };
  }
  componentDidMount() {}
  componentWillReceiveProps(nextProps) {
    this.setState(
      {
        params: {
          ...this.state.params,
          password: nextProps.password
        }
      },
      () => this.selectRole()
    );
  }
  onSuccess = () => {
    this.setState({
      loading: false,
      params: {
        userName: "",
        givenName: "",
        familyName: "",
        typeOfAccount: "Commercial",
        roleID: "",
        password: "",
        email: "",
        contactByEmail: false,
        encryptionActive: false
      }
    });
  };
  onFail = error => {
    this.setState({
      loading: false,
      error: {
        status: true,
        message: error
      }
    });
  };
  onClose = () => {
    this.props.onToggle(false);
  };
  onSubmit = () => {
    this.setState(
      {
        loading: true
      },
      () => {
        this.props.onSubmit(this.state.params, this.onSuccess, this.onFail);
      }
    );
  };
  onChange = e => {
    const key = e.target.name;
    const value = e.target.value;
    this.setState({
      params: {
        ...this.state.params,
        [key]: value
      }
    });
  };
  onCheck = e => {
    const key = e.target.name;
    this.setState({
      params: {
        ...this.state.params,
        [key]: e.target.checked
      }
    });
  };
  selectRole = () => {
    const { profile } = this.props;
    switch (this.state.params.typeOfAccount) {
      case "Private":
        this.setState({
          readerRole: false,
          userRole: true,
          adminRole: false,
          superadminRole: false,
          params: { ...this.state.params, roleID: "" }
        });
        return;
      case "Test":
        this.setState({
          readerRole: false,
          userRole: true,
          adminRole: false,
          superadminRole: false,
          params: { ...this.state.params, roleID: "" }
        });
        return;
      case "Commercial":
        this.setState({
          readerRole: true,
          userRole: true,
          adminRole: _.indexOf(permission.user.addAdmin, profile.roleID) !== -1,
          superadminRole: false,
          params: { ...this.state.params, roleID: "" }
        });
        return;
      case "CSBG":
        this.setState({
          readerRole: true,
          userRole: true,
          adminRole: _.indexOf(permission.user.addAdmin, profile.roleID) !== -1,
          superadminRole: true,
          params: { ...this.state.params, roleID: "" }
        });
        return;
      default:
        this.setState({
          readerRole: false,
          userRole: false,
          adminRole: false,
          superadminRole: false,
          params: { ...this.state.params, roleID: "" }
        });
    }
  };

  render() {
    const { status } = this.props;
    let enableSubmit =
      this.state.params.userName !== "" &&
      this.state.params.givenName !== "" &&
      this.state.params.familyName !== "" &&
      this.state.params.typeOfAccount !== "" &&
      this.state.params.roleID !== "" &&
      this.state.params.password !== "" &&
      this.state.params.email !== "";
    return (
      <div>
        {this.state.loading ? (
          <CircularProgress />
        ) : (
          <Dialog open={status} onClose={this.onClose} fullWidth>
            <DialogTitle>
              <h3>Add new user</h3>
              <div>
                {this.state.error.status && (
                  <p style={{ color: "#F44336" }}>{this.state.error.message}</p>
                )}
              </div>
            </DialogTitle>
            <DialogContent>
              <div>
                <TextField
                  required
                  name="userName"
                  label="Username"
                  margin="normal"
                  fullWidth
                  value={this.state.params.userName}
                  onChange={e => this.onChange(e)}
                />
              </div>
              <div>
                <TextField
                  required
                  name="givenName"
                  label="Given Name"
                  margin="normal"
                  fullWidth
                  value={this.state.params.givenName}
                  onChange={e => this.onChange(e)}
                />
              </div>
              <div>
                <TextField
                  required
                  name="familyName"
                  label="Family Name"
                  margin="normal"
                  fullWidth
                  value={this.state.params.familyName}
                  onChange={e => this.onChange(e)}
                />
              </div>
              <div>
                <FormControl margin="normal" fullWidth>
                  <InputLabel>Role</InputLabel>
                  <NativeSelect
                    name="roleID"
                    value={this.state.params.roleID}
                    onChange={e => this.onChange(e)}
                  >
                    <option value={""}></option>
                    <option disabled={!this.state.readerRole} value={4}>
                      Reader
                    </option>
                    <option disabled={!this.state.userRole} value={3}>
                      User
                    </option>
                    <option disabled={!this.state.adminRole} value={2}>
                      Admin
                    </option>
                    <option disabled={!this.state.superadminRole} value={1}>
                      Superadmin
                    </option>
                  </NativeSelect>
                </FormControl>
              </div>
              <div>
                <TextField
                  required
                  name="password"
                  label="Password"
                  margin="normal"
                  fullWidth
                  value={this.state.params.password}
                  onChange={e => this.onChange(e)}
                />
              </div>
              <div>
                <TextField
                  required
                  type="email"
                  name="email"
                  label="Email"
                  margin="normal"
                  fullWidth
                  value={this.state.params.email}
                  onChange={e => this.onChange(e)}
                />
              </div>
              <div>
                <FormControl margin="normal" fullWidth>
                  <FormLabel>Please contact me by email</FormLabel>
                  <FormControlLabel
                    control={
                      <Checkbox name="contactByEmail" onChange={this.onCheck} />
                    }
                  />
                </FormControl>
              </div>
              <div>
                <FormControl margin="normal" fullWidth>
                  <FormLabel>Encryption active</FormLabel>
                  <FormControlLabel
                    control={
                      <Checkbox
                        name="encryptionActive"
                        onChange={this.onCheck}
                      />
                    }
                  />
                </FormControl>
              </div>
            </DialogContent>

            {this.state.loading ? (
              <CircularProgress />
            ) : (
              <DialogActions>
                <Button onClick={this.onClose} color="primary">
                  Cancel
                </Button>
                <Button
                  disabled={!enableSubmit}
                  onClick={this.onSubmit}
                  color="primary"
                  autoFocus
                >
                  Submit
                </Button>
              </DialogActions>
            )}
          </Dialog>
        )}
      </div>
    );
  }
}

const mapStateToProps = state => {
  return {
    profile: state.Auth.profile
  };
};

const mapDispatchToProps = {};
export default connect(
  mapStateToProps,
  mapDispatchToProps
)(Form);
