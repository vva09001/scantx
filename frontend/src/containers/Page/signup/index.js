import React, { Component } from "react";
import { connect } from "react-redux";
import signinImg from "../../../images/signin.svg";
import TextField from "../../../components/uielements/textfield";
import Scrollbars from "../../../components/utility/customScrollBar";
import SignUpStyleWrapper from "./signup.style";
import { Checkbox } from "./signup.style";
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
import { authActions, userActions } from "redux/actions";
import { CircularProgress } from "components/uielements/progress";
import RegisterSuccess from "./RegisterSuccess";
import { generatePassword } from "helpers/user";

class SignUp extends Component {
  constructor(props) {
    super(props);
    this.state = {
      redirectToReferrer: false,
      loading: false,
      error: {
        status: false,
        message: ""
      },
      registerSuccess: false,
      readerRole: false,
      userRole: false,
      adminRole: false,
      superadminRole: false,
      submitCompany: false,
      params: {
        userName: "",
        givenName: "",
        familyName: "",
        typeOfAccount: "",
        roleID: "",
        password: "",
        email: "",
        contactByEmail: false,
        encryptionActive: false,

        name: "",
        address: "",
        status: ""
      }
    };
  }

  componentDidMount() {
    this.setState({
      params: {
        ...this.state.params,
        password: generatePassword()
      }
    });
  }

  componentWillReceiveProps(nextProps) {
    if (
      this.props.isLoggedIn !== nextProps.isLoggedIn &&
      nextProps.isLoggedIn === true
    ) {
      this.setState({ redirectToReferrer: true });
    }
  }
  handleLogin = () => {
    const { login } = this.props;
    login();
    this.props.history.push("/dashboard");
  };

  onSuccess = () => {
    this.setState({
      loading: false
    });
  };

  onRegisterSuccess = () => {
    this.setState({
      loading: false,
      registerSuccess: true
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

  onSubmit = () => {
    this.setState(
      {
        loading: true,
        params: {
          ...this.state.params,
          name:
            this.state.params.typeOfAccount === "Test" ||
            this.state.params.typeOfAccount === "Private"
              ? this.state.params.userName
              : this.state.params.name
        }
      },
      () => {
        this.props.registerUser(
          this.state.params,
          this.onRegisterSuccess,
          this.onFail
        );
      }
    );
  };

  onCloseCompany = () => {
    this.setState({
      params: {
        ...this.state.params,
        typeOfAccount: ""
      }
    });
  };

  submitCompany = () => {
    this.setState({
      submitCompany: true
    });
  };

  onChange = e => {
    const key = e.target.name;
    const value = e.target.value;
    this.setState(
      {
        params: {
          ...this.state.params,
          [key]: value
        }
      },
      () => {
        if (key === "typeOfAccount") {
          this.selectRole();
        }
      }
    );
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
    switch (this.state.params.typeOfAccount) {
      case "Private":
        this.setState({
          readerRole: false,
          userRole: true,
          adminRole: false,
          superadminRole: false,
          submitCompany: false,
          params: {
            ...this.state.params,
            roleID: "",
            name: "",
            address: "",
            status: ""
          }
        });
        return;
      case "Test":
        this.setState({
          readerRole: false,
          userRole: true,
          adminRole: false,
          superadminRole: false,
          submitCompany: false,
          params: {
            ...this.state.params,
            roleID: "",
            name: "",
            address: "",
            status: ""
          }
        });
        return;
      case "Commercial":
        this.setState({
          readerRole: true,
          userRole: true,
          adminRole: true,
          superadminRole: false,
          params: { ...this.state.params, roleID: "" }
        });
        return;
      case "CSBG":
        this.setState({
          readerRole: true,
          userRole: true,
          adminRole: true,
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
    let enableSubmit =
      this.state.params.userName !== "" &&
      this.state.params.givenName !== "" &&
      this.state.params.familyName !== "" &&
      this.state.params.typeOfAccount !== "" &&
      this.state.params.roleID !== "" &&
      this.state.params.password !== "" &&
      this.state.params.email !== "";

    let enableSubmitCompany = this.state.params.name !== "";

    return (
      <SignUpStyleWrapper className="mateSignUpPage">
        <div className="mateSignInPageImgPart">
          <div className="mateSignInPageImg">
            <img src={signinImg} alt="Kiwi standing on oval" />
          </div>
        </div>

        <div className="mateSignInPageContent">
          <Scrollbars style={{ height: "100%" }}>
            {/* User Form */}
            <Dialog open={true} fullWidth>
              <DialogTitle>
                <h3>Register</h3>
                <div>
                  {this.state.error.status && (
                    <p style={{ color: "#F44336" }}>
                      {this.state.error.message}
                    </p>
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
                    <InputLabel>Type of Account</InputLabel>
                    <NativeSelect
                      required
                      name="typeOfAccount"
                      value={this.state.params.typeOfAccount}
                      onChange={e => this.onChange(e)}
                    >
                      <option value={""}></option>
                      <option value={"Commercial"}>Commercial</option>
                      <option value={"Test"}>Test</option>
                      <option value={"Private"}>Private</option>
                    </NativeSelect>
                  </FormControl>
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
                        <Checkbox
                          name="contactByEmail"
                          onChange={this.onCheck}
                        />
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
                  <Button
                    onClick={() => this.props.history.push("./")}
                    color="primary"
                  >
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

            {/* Company Form */}
            <Dialog
              open={
                this.state.params.typeOfAccount === "Commercial" &&
                !this.state.submitCompany
              }
              onClose={this.onCloseCompany}
            >
              {this.state.loading ? (
                <CircularProgress />
              ) : (
                <React.Fragment>
                  <DialogTitle>{"Create your company"}</DialogTitle>
                  <DialogContent>
                    <div>
                      <TextField
                        required
                        label="Name"
                        margin="normal"
                        fullWidth
                        name="name"
                        value={this.state.params.name}
                        onChange={e => this.onChange(e)}
                      />
                    </div>
                    <div>
                      <TextField
                        label="Address"
                        margin="normal"
                        fullWidth
                        name="address"
                        value={this.state.params.address}
                        onChange={e => this.onChange(e)}
                      />
                    </div>
                    <div>
                      <TextField
                        label="Status"
                        margin="normal"
                        fullWidth
                        name="status"
                        value={this.state.params.status}
                        onChange={e => this.onChange(e)}
                      />
                    </div>
                  </DialogContent>
                  <DialogActions>
                    <Button
                      onClick={() => this.onCloseCompany()}
                      color="primary"
                    >
                      Cancel
                    </Button>
                    <Button
                      disabled={!enableSubmitCompany}
                      onClick={() => this.submitCompany()}
                      color="primary"
                      autoFocus
                    >
                      Submit
                    </Button>
                  </DialogActions>
                </React.Fragment>
              )}
            </Dialog>

            {/* Register Success */}
            <RegisterSuccess
              status={this.state.registerSuccess}
              onSubmit={() => this.props.history.push("./")}
            />
          </Scrollbars>
        </div>
      </SignUpStyleWrapper>
    );
  }
}
const mapSateToProps = state => {
  return {
    isLoggedIn: state.Auth.idToken !== null ? true : false
  };
};

const mapDispatchToProps = {
  register: authActions.register,
  registerUser: userActions.registerUser
};
export default connect(
  mapSateToProps,
  mapDispatchToProps
)(SignUp);
