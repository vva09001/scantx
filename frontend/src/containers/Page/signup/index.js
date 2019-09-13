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
import { authActions, userActions, companyActions } from "redux/actions";
import { CircularProgress } from "components/uielements/progress";
import _ from "lodash";

const generatePassword = () => {
  const length = 8;
  const charset = "23456789abcdefghmnpqrstuvwxyzABCDEFGHLMNPQRSTUVWXYZ";
  const n = charset.length;
  let password = "";
  for (let i = 0; i < length; ++i) {
    password += charset.charAt(Math.floor(Math.random() * n));
  }
  return password;
};

class SignUp extends Component {
  constructor(props) {
    super(props);
    this.state = {
      redirectToReferrer: false,
      loading: false,
      readerRole: false,
      userRole: false,
      adminRole: false,
      superadminRole: false,
      params: {
        userName: "",
        givenName: "",
        familyName: "",
        typeOfAccount: "",
        roleId: "",
        cid: "",
        password: "",
        email: "",
        contactByEmail: false,
        encryptionActive: false
      }
    };
  }

  componentDidMount() {
    this.props.getCompanies(this.onSuccess, this.onSuccess);
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

  onSubmit = () => {
    this.setState(
      {
        loading: true
      },
      () => {
        this.props.addUser(this.state.params, this.onSuccess, this.onSuccess);
      }
    );
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
          params: { ...this.state.params, roleId: "" }
        });
        return;
      case "Test":
        this.setState({
          readerRole: false,
          userRole: true,
          adminRole: false,
          superadminRole: false,
          params: { ...this.state.params, roleId: "" }
        });
        return;
      case "Commercial":
        this.setState({
          readerRole: true,
          userRole: true,
          adminRole: true,
          superadminRole: false,
          params: { ...this.state.params, roleId: "" }
        });
        return;
      case "CSBG":
        this.setState({
          readerRole: true,
          userRole: true,
          adminRole: true,
          superadminRole: true,
          params: { ...this.state.params, roleId: "" }
        });
        return;
      default:
        this.setState({
          readerRole: false,
          userRole: false,
          adminRole: false,
          superadminRole: false,
          params: { ...this.state.params, roleId: "" }
        });
    }
  };
  renderCompanies = () => {
    if (
      this.state.params.typeOfAccount === "Test" ||
      this.state.params.typeOfAccount === "Private"
    ) {
      return <option value={null}>{this.state.params.userName}</option>;
    } else if (this.state.params.typeOfAccount === "Commercial") {
      return _.map(
        _.filter(this.props.companies, item => item.cid === this.props.cid),
        item => {
          return (
            <option key={item.cid} value={item.cid}>
              {item.name}
            </option>
          );
        }
      );
    } else {
      return _.map(this.props.companies, item => {
        return (
          <option key={item.cid} value={item.cid}>
            {item.name}
          </option>
        );
      });
    }
  };

  render() {
    let enableSubmit =
      this.state.params.userName !== "" &&
      this.state.params.givenName !== "" &&
      this.state.params.familyName !== "" &&
      this.state.params.typeOfAccount !== "" &&
      this.state.params.roleId !== "" &&
      this.state.params.cid !== "" &&
      this.state.params.password !== "" &&
      this.state.params.email !== "";
    return (
      <SignUpStyleWrapper className="mateSignUpPage">
        <div className="mateSignInPageImgPart">
          <div className="mateSignInPageImg">
            <img src={signinImg} alt="Kiwi standing on oval" />
          </div>
        </div>

        <div className="mateSignInPageContent">
          <Scrollbars style={{ height: "100%" }}>
            <Dialog open={true} fullWidth>
              <DialogTitle>{"Add new user"}</DialogTitle>
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
                      <option value={"CSBG"}>CSBG</option>
                    </NativeSelect>
                  </FormControl>
                </div>
                <div>
                  <FormControl margin="normal" fullWidth>
                    <InputLabel>Role</InputLabel>
                    <NativeSelect
                      name="roleId"
                      value={this.state.params.roleId}
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
                  <FormControl margin="normal" fullWidth>
                    <InputLabel>Company</InputLabel>
                    <NativeSelect
                      name="cid"
                      value={this.state.params.cid}
                      onChange={e => this.onChange(e)}
                    >
                      <option value={""}></option>
                      {this.renderCompanies()}
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
          </Scrollbars>
        </div>
      </SignUpStyleWrapper>
    );
  }
}
const mapSateToProps = state => {
  return {
    companies: state.Company.list,
    isLoggedIn: state.Auth.idToken !== null ? true : false
  };
};

const mapDispatchToProps = {
  register: authActions.register,
  addUser: userActions.addUser,
  getCompanies: companyActions.get
};
export default connect(
  mapSateToProps,
  mapDispatchToProps
)(SignUp);
